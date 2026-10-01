using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ReRoute.Api.DTOs;

namespace ReRoute.Api.Tests;

public class ControllersSmokeTests : IClassFixture<ReRouteApiFactory>
{
    private readonly HttpClient _client;

    public ControllersSmokeTests(ReRouteApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task AutenticarAsync()
    {
        var login = new LoginRequestDto
        {
            Email = "joao@email.com",
            Senha = "senha1"
        };

        var response = await _client.PostAsJsonAsync("/api/auth/login", login);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.Token);
    }

    [Fact]
    public async Task ParticipantesController_Listar_Retorna200()
    {
        await AutenticarAsync();

        var response = await _client.GetAsync("/api/participantes?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DoacoesController_Listar_Retorna200()
    {
        var response = await _client.GetAsync("/api/doacoes?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SolicitacoesController_Listar_Retorna200()
    {
        var response = await _client.GetAsync("/api/solicitacoes?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task EntregasController_Listar_Retorna200()
    {
        var response = await _client.GetAsync("/api/entregas?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task RegistrosESGController_Listar_Retorna200()
    {
        await AutenticarAsync();

        var response = await _client.GetAsync("/api/registros?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}