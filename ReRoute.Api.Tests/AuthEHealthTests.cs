using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReRoute.Api.DTOs;

namespace ReRoute.Api.Tests;

public class AuthEHealthTests : IClassFixture<ReRouteApiFactory>
{
    private readonly ReRouteApiFactory _factory;

    public AuthEHealthTests(ReRouteApiFactory factory)
    {
        _factory = factory;
    }

    private async Task<string> ObterTokenAsync(HttpClient client, string email, string senha)
    {
        var response = await client.PostAsJsonAsync("/api/auth/login", new LoginRequestDto { Email = email, Senha = senha });
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        return result!.Token;
    }

    [Fact]
    public async Task Health_Retorna200()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Login_UsuarioDoSeed_RetornaToken()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login",
            new LoginRequestDto { Email = "maria@email.com", Senha = "senha2" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
        Assert.False(string.IsNullOrEmpty(result!.Token));
        Assert.Equal("RECEPTOR", result.TipoPerfil);
    }

    [Fact]
    public async Task Login_SenhaErrada_Retorna401()
    {
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/auth/login",
            new LoginRequestDto { Email = "joao@email.com", Senha = "errada" });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Participantes_SemToken_Retorna401()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/participantes?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CriarDoacao_ComTokenDeDoador_Retorna201()
    {
        var client = _factory.CreateClient();
        var token = await ObterTokenAsync(client, "joao@email.com", "senha1");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var doacao = new DoacaoCreateDto
        {
            ParticipantId = 1,
            NomeItem = "Cobertores",
            Descricao = "Cobertores de casal usados",
            Tamanho = "MEDIO",
            LocalRetirada = "Rua F, 303",
            Status = "DISPONIVEL"
        };

        var response = await client.PostAsJsonAsync("/api/doacoes", doacao);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CriarDoacao_ComTokenDeReceptor_Retorna403()
    {
        var client = _factory.CreateClient();
        var token = await ObterTokenAsync(client, "maria@email.com", "senha2");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var doacao = new DoacaoCreateDto
        {
            ParticipantId = 2,
            NomeItem = "Sapatos",
            Descricao = "Par de sapatos",
            Tamanho = "PEQUENO",
            LocalRetirada = "Rua G, 404"
        };

        var response = await client.PostAsJsonAsync("/api/doacoes", doacao);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}
