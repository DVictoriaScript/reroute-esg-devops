# Projeto - Cidades ESGInteligentes

Projeto **ReRoute**: API REST em ASP.NET Core 8 para uma plataforma de economia circular. A ideia é conectar doadores, receptores e transportadores em um fluxo de doação e registrar o impacto ambiental (ESG) de cada doação.

**Fluxo:** Doação → Solicitação → Entrega → Registro ESG

## Integrantes

| Nome | RM |
| --- | --- |
| Daphne Victoria Pollini Amadeu | RM562717 |
| Paula Cardoso da Cruz | RM562406 |
| Kenny Lemes Brisola | RM564336 |

---

## Como executar localmente com Docker

> Esta seção será completada com o `docker-compose.yml` (responsável: Paula).

Enquanto isso, dá para rodar a API direto com o .NET 8 SDK.

1. Copie o arquivo `.env.example` e preencha os valores (connection string do Oracle e chave JWT). As mesmas variáveis podem ser definidas no terminal:

```bash
export OracleConnection__ConnectionString="User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
export Jwt__SecretKey="uma-chave-com-pelo-menos-32-caracteres"
```

No PowerShell:

```powershell
$env:OracleConnection__ConnectionString="User Id=SEU_RM;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
$env:Jwt__SecretKey="uma-chave-com-pelo-menos-32-caracteres"
```

2. Rode a API:

```bash
dotnet restore
dotnet run --project ReRoute.Api
```

3. Acesse o Swagger em `http://localhost:5110/swagger` e o health check em `http://localhost:5110/health`.

Para rodar os testes (não precisam de banco, usam EF Core InMemory):

```bash
dotnet test
```

Usuários de exemplo criados pelo seed: `joao@email.com` / `senha1` (DOADOR), `maria@email.com` / `senha2` (RECEPTOR), `carlos@email.com` / `senha3` (TRANSPORTADOR).

---

## Pipeline CI/CD

### Ferramenta

Usamos o **GitHub Actions**. O workflow fica em `.github/workflows/ci-cd.yml` e roda automaticamente a cada `push` ou `pull request` nas branches `main` e `develop`. A imagem Docker vai para o **Docker Hub** e a aplicação roda no **Azure Web App** (App Service com container Linux), com dois ambientes: **staging (homologação)** e **production**.

### Etapas

O pipeline tem 4 jobs, ligados com `needs` (um só começa se o anterior der certo):

1. **build-test (Build e testes)**
   - faz o checkout do código e instala o .NET 8;
   - faz o `dotnet restore` e o `dotnet build` em modo Release;
   - roda os testes xUnit com `dotnet test` e gera um relatório `.trx`;
   - publica o relatório como artefato (`resultado-testes`), que pode ser baixado na aba Actions.

   Se algum teste falhar, o pipeline para aqui e nada é publicado.

2. **docker (Build e push da imagem no Docker Hub)**
   - só roda em `push` (em pull request apenas build e testes são executados);
   - faz login no Docker Hub com os secrets `DOCKERHUB_USERNAME` e `DOCKERHUB_TOKEN`;
   - gera a imagem a partir do `Dockerfile` da raiz e envia para `<usuario-dockerhub>/reroute-api`;
   - cada imagem recebe duas tags: o SHA do commit (`${{ github.sha }}`, para saber exatamente qual versão está rodando) e o nome do ambiente (`staging` para a `develop` e `latest` para a `main`).

3. **deploy-staging (Deploy em staging/homologação)**
   - roda quando há push na `develop`;
   - usa o environment `staging` do GitHub;
   - usa a action `azure/webapps-deploy@v2` com o publish profile do Web App de staging para trocar a imagem do container pela imagem com a tag do commit;
   - depois testa o endpoint `/health` da aplicação.

4. **deploy-production (Deploy em produção)**
   - roda quando há push na `main`;
   - usa o environment `production` do GitHub;
   - faz o mesmo processo do staging, apontando para o Web App de produção.

### Estratégia de branches

| Branch | O que acontece | Ambiente |
| --- | --- | --- |
| `feature/*` (pull request para `develop`) | só build e testes | nenhum |
| `develop` | build, testes, imagem `:staging` e deploy | staging (homologação) |
| `main` | build, testes, imagem `:latest` e deploy | produção |
| pull request da `develop` para a `main` | só build e testes | nenhum |

O fluxo de trabalho é: cada funcionalidade é feita numa branch `feature/...` criada a partir da `develop`. Quando termina, abrimos um pull request para a `develop`; o pipeline roda build e testes e, se algum teste falhar, o merge não deve ser feito. Depois do merge, a `develop` sobe para staging. Quando a versão está validada em staging, abrimos um pull request da `develop` para a `main`, e o merge faz o deploy em produção.

Na prática isso separa as duas partes do CI/CD:

- **Integração contínua (CI):** todo código novo é compilado e testado automaticamente antes de entrar na `develop`/`main`.
- **Entrega contínua (CD):** depois dos testes, a imagem é gerada e publicada sozinha em staging e em produção. Como o deploy em produção também é automático após o merge na `main`, o nosso fluxo fica mais perto de *implantação contínua* (continuous deployment). Se o environment `production` tiver revisor obrigatório no GitHub (Settings > Environments > Required reviewers), o último passo passa a ser uma aprovação manual, que é o modelo de entrega contínua.

### Secrets e variáveis

Nenhuma senha fica no código. O `appsettings.json` vai com os campos sensíveis vazios e a aplicação lê tudo de variáveis de ambiente (o ASP.NET converte `__` em `:`, então `Jwt__SecretKey` vira `Jwt:SecretKey`).

No GitHub:

| Nome | Onde | Tipo | Para que serve |
| --- | --- | --- | --- |
| `DOCKERHUB_USERNAME` | Settings > Secrets and variables > Actions (repositório) | secret | usuário do Docker Hub |
| `DOCKERHUB_TOKEN` | Settings > Secrets and variables > Actions (repositório) | secret | access token do Docker Hub (não é a senha) |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Settings > Environments > `staging` e `production` | secret | conteúdo do publish profile baixado de cada Web App |
| `AZURE_WEBAPP_NAME` | Settings > Environments > `staging` e `production` | variable | nome do Web App de cada ambiente |
| `APP_URL` | Settings > Environments > `staging` e `production` | variable | endereço público do Web App (ex.: `https://<nome>.azurewebsites.net`), usado para testar o `/health` |

No Azure (Web App > Configuração > Configurações do aplicativo) ficam as variáveis da aplicação: `WEBSITES_PORT=8080`, `OracleConnection__ConnectionString`, `Jwt__SecretKey`, `Jwt__Issuer`, `Jwt__Audience` e `ASPNETCORE_ENVIRONMENT`.

O passo a passo de configuração está em [`docs/SETUP_AZURE_DOCKERHUB.md`](docs/SETUP_AZURE_DOCKERHUB.md).

### Como funciona o deploy

Depois que a imagem é publicada no Docker Hub, o job de deploy chama a action `azure/webapps-deploy@v2`, informando o nome do Web App, o publish profile e a imagem com a tag do commit. O Azure baixa essa imagem do Docker Hub e reinicia o container com a versão nova. Em seguida o pipeline espera e faz requisições no `APP_URL/health` a cada 15 segundos, por até 5 minutos (no plano gratuito o container pode demorar para subir). Se o `/health` responder 200, o deploy é considerado ok; se não responder, o job falha.

Como cada ambiente tem um único slot, a troca de versão substitui o container que está rodando, o que fica mais próximo do *recreate*. Para fazer um blue-green de verdade no Azure seria preciso usar *deployment slots*, que só existem a partir do plano Standard.

Se os secrets do Docker Hub ou do Azure ainda não estiverem configurados, o job mostra um aviso e pula a etapa sem quebrar o pipeline. Assim o build e os testes continuam funcionando mesmo antes dos ambientes estarem prontos.

### Testes automatizados

Os testes ficam em `ReRoute.Api.Tests` e usam xUnit com `WebApplicationFactory`, subindo a API em memória com banco EF Core InMemory e uma chave JWT só de teste. Por isso o pipeline não precisa de nenhum secret para rodar os testes. Hoje são 11 testes:

- listagem dos 5 controllers (participantes, doações, solicitações, entregas e registros ESG);
- `/health` retornando 200;
- login com usuário do seed retornando token;
- login com senha errada retornando 401;
- rota protegida sem token retornando 401;
- cadastro de doação com token de DOADOR retornando 201;
- cadastro de doação com token de RECEPTOR retornando 403.

Pensando na pirâmide de testes, esses testes ficam na camada de **integração**: eles chamam a API por HTTP e passam por controller, serviço, repositório e banco em memória, verificando só a entrada e a saída (caixa preta). O arquivo `.trx` publicado como artefato funciona como o relatório de teste de cada execução do pipeline.

---

## Containerização

> Seção em construção (responsável: Paula). Aqui vão o Dockerfile explicado, o `docker-compose.yml` e as estratégias adotadas.

Resumo atual: o `Dockerfile` usa build em duas etapas (imagem `sdk:8.0` para compilar e `aspnet:8.0` para rodar) e a API escuta na porta `8080`.

```bash
docker build -t reroute-api .
docker run -p 8080:8080 --env-file .env reroute-api
```

---

## Prints do funcionamento

> Em construção. Aqui vão os prints do pipeline rodando no GitHub Actions, da imagem no Docker Hub e da aplicação rodando no Azure Web App em staging (homologação) e produção.

---

## Tecnologias utilizadas

- C# / ASP.NET Core 8 (Web API)
- Entity Framework Core 8 com Oracle (`Oracle.EntityFrameworkCore`)
- Oracle Database (FIAP)
- Autenticação JWT Bearer com perfis DOADOR, RECEPTOR e TRANSPORTADOR
- Swagger / OpenAPI
- xUnit, `Microsoft.AspNetCore.Mvc.Testing` e EF Core InMemory
- Docker
- GitHub Actions
- Docker Hub (registro das imagens)
- Microsoft Azure: Azure Web App / App Service com container Linux (ambientes staging/homologação e produção)
- Postman (collection em `docs/postman/`)

---

## Estrutura do repositório

```text
.github/workflows/ci-cd.yml   pipeline CI/CD
ReRoute.Api/                  código da API
ReRoute.Api.Tests/            testes xUnit
docs/postman/                 collection do Postman
docs/SETUP_AZURE_DOCKERHUB.md passo a passo do Docker Hub, Azure Web App e GitHub Environments
Dockerfile
.env.example                  variáveis de ambiente necessárias
```

---

## Checklist de entrega

| Item | OK |
| --- | --- |
| Projeto compactado em .ZIP com estrutura organizada | [ ] |
| Dockerfile funcional | [ ] |
| docker-compose.yml ou arquivos Kubernetes | [ ] |
| Pipeline com etapas de build, teste e deploy | [ ] |
| README.md com instruções e prints | [ ] |
| Documentação técnica com evidências (PDF ou PPT) | [ ] |
| Deploy realizado nos ambientes staging (homologação) e produção | [ ] |
