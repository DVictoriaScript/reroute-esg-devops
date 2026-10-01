# Configuração do Docker Hub, Azure Web App e GitHub Environments

Este guia explica como deixar o pipeline (`.github/workflows/ci-cd.yml`) publicando a imagem no Docker Hub e fazendo o deploy nos dois ambientes do Azure: **staging (homologação)** e **production**. Basta fazer uma vez.

Enquanto alguma etapa não estiver configurada, o pipeline mostra um aviso amarelo e pula a parte de imagem ou de deploy, sem ficar vermelho. Build e testes rodam sempre.

---

## 1. Docker Hub (conta + access token)

1. Crie uma conta em <https://hub.docker.com> (pode ser de qualquer pessoa do grupo).
2. Vá em **Account settings > Personal access tokens > Generate new token**.
   - Descrição: `github-actions-reroute`
   - Permissão: **Read & Write**
   - Copie o token gerado. Ele só aparece uma vez.
3. Não precisa criar o repositório da imagem: no primeiro push, o pipeline cria `<seu-usuario>/reroute-api`. Deixe esse repositório **público**, assim o Azure consegue baixar a imagem sem senha.

## 2. GitHub: secrets do Docker Hub (nível do repositório)

No repositório: **Settings > Secrets and variables > Actions > New repository secret**

| Nome | Valor |
| --- | --- |
| `DOCKERHUB_USERNAME` | usuário do Docker Hub |
| `DOCKERHUB_TOKEN` | token criado no passo 1 (não a senha) |

Depois disso, um push na `develop` ou na `main` já publica a imagem no Docker Hub (tags `<sha do commit>` e `staging` ou `latest`).

## 3. Azure: criar os dois Web Apps (portal)

Pré-requisito: conta Azure com o e-mail @fiap.com.br e o benefício **Azure for Students** ativado (como na aula de Azure DevOps).

Faça os passos abaixo **duas vezes**, uma para cada ambiente:

| | staging (homologação) | production |
| --- | --- | --- |
| Nome do Web App (exemplo, tem que ser único no Azure) | `reroute-api-staging-<rm>` | `reroute-api-prod-<rm>` |

1. No [portal do Azure](https://portal.azure.com), procure **App Services** e clique em **Create > Web App**.
2. Aba **Basics**:
   - **Subscription:** Azure for Students
   - **Resource Group:** crie um, por exemplo `rg-reroute` (pode ser o mesmo para os dois)
   - **Name:** o nome da tabela acima
   - **Publish:** **Container**
   - **Operating System:** **Linux**
   - **Region:** **Brazil South** (como na aula). Se aparecer erro de região não permitida (`RequestDisallowedByAzure`), é porque a assinatura de estudante só libera algumas regiões. Veja quais em **Policy > Assignments > "Allowed resource deployment regions"** e use uma delas.
   - **Pricing plan:** crie um plano Linux e escolha **Free F1** (em *Change size* / *Explore pricing plans* > *Dev/Test* > **F1**). O mesmo plano pode ser usado pelos dois Web Apps. Se precisarem de mais desempenho, o **Basic B1** é pago e consome o crédito de estudante.
3. Aba **Container**: pode deixar a imagem de exemplo (Quickstart) ou apontar para o Docker Hub (*Other container registries*, acesso **Public**, imagem `<usuario>/reroute-api:latest`). Na primeira execução, o pipeline troca pela imagem certa.
4. **Review + create > Create** e depois **Go to resource**.

### 3.1 Configurações do aplicativo (variáveis de ambiente)

No Web App: **Settings > Environment variables** (em versões antigas do portal: **Configuration > Application settings**). Adicione:

| Nome | Valor |
| --- | --- |
| `WEBSITES_PORT` | `8080` (a API escuta na 8080; o Azure espera a porta 80 se essa variável não existir) |
| `ASPNETCORE_URLS` | `http://+:8080` (já está no Dockerfile; colocar aqui é opcional) |
| `OracleConnection__ConnectionString` | `User Id=<schema>;Password=<senha>;Data Source=oracle.fiap.com.br:1521/ORCL` (um schema diferente para cada ambiente) |
| `Jwt__SecretKey` | uma chave com pelo menos 32 caracteres (diferente em cada ambiente) |
| `Jwt__Issuer` | `ReRoute.Api` |
| `Jwt__Audience` | `ReRoute.Client` |
| `ASPNETCORE_ENVIRONMENT` | `Development` no staging (o Swagger só aparece nesse modo, bom para os prints) e `Production` na produção |
| `WEBSITE_WEBDEPLOY_USE_SCM` | `true` (é obrigatório em Web App Linux **antes** de baixar o publish profile) |

Clique em **Apply/Save** e confirme o reinício.

### 3.2 Liberar a autenticação básica e baixar o publish profile

O deploy com publish profile só funciona com a autenticação básica (SCM) ligada:

1. No Web App: **Configuration > General settings** e ligue **SCM Basic Auth Publishing Credentials** (**On**). Salve.
2. Volte em **Overview** e clique em **Download publish profile**. Vem um arquivo `.PublishSettings` (um XML).
3. Abra o arquivo num editor de texto e copie **todo** o conteúdo. Ele vai para o GitHub no próximo passo. Não mande esse arquivo para o repositório.

Repita os passos 3, 3.1 e 3.2 para o outro ambiente.

## 4. GitHub: environments `staging` e `production`

No repositório: **Settings > Environments > New environment**. Crie `staging` e `production` (se o pipeline já tiver rodado, eles já existem). Em **cada** environment:

| Nome | Tipo | Valor |
| --- | --- | --- |
| `AZURE_WEBAPP_PUBLISH_PROFILE` | Environment secret | conteúdo inteiro do publish profile **daquele** Web App |
| `AZURE_WEBAPP_NAME` | Environment variable | nome do Web App (ex.: `reroute-api-staging-<rm>`) |
| `APP_URL` | Environment variable | `https://<nome-do-web-app>.azurewebsites.net` (aparece como *Default domain* no Overview) |

Opcional: em `production`, marque **Required reviewers** para exigir aprovação antes do deploy (no plano gratuito do GitHub isso só funciona em repositório público).

> O repositório deve ser **público**: no plano gratuito do GitHub, os secrets e variáveis de environment só funcionam em repositório público. Não há nenhuma senha no código.

## 5. Testar

1. Faça um push (ou merge) na `develop`: na aba **Actions**, os jobs `build-test`, `docker` e `deploy-staging` devem ficar verdes, e o último passo mostra `Healthy` vindo de `APP_URL/health`.
2. Abra um pull request da `develop` para a `main` e faça o merge: roda o `deploy-production`.
3. Para os prints, confira:
   - a aba Actions;
   - a imagem no Docker Hub, com as tags;
   - o Overview e o Deployment Center de cada Web App;
   - `https://<staging>.azurewebsites.net/swagger` e `/health`.

## Problemas comuns

- **Container não responde / timeout no /health:** confira `WEBSITES_PORT=8080` e veja os logs em **Log stream** do Web App. No plano F1 o container pode demorar alguns minutos para subir na primeira vez.
- **Erro de autenticação no deploy:** a autenticação básica (SCM) está desligada ou o publish profile foi baixado antes de criar `WEBSITE_WEBDEPLOY_USE_SCM=true`. Ligue a opção, baixe o arquivo de novo e atualize o secret.
- **API cai logo ao iniciar:** normalmente é a connection string do Oracle (a API acessa o banco ao iniciar, para criar os dados de exemplo). Confira usuário, senha e servidor.
