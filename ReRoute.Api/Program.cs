using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using ReRoute.Api.Data;
using ReRoute.Api.Middleware;
using ReRoute.Api.Repositories;
using ReRoute.Api.Services;
using ReRoute.Api.Configuration;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// =============================================================================
// ReRoute ESG API - ASP.NET Core 8 (FIAP)
// Arquitetura acadêmica: Controller → Service → Repository → EF Core
// Projeto Java Spring Boot usado apenas como referência de domínio.
// =============================================================================

builder.Services.AddControllers();

// Documentação Swagger/OpenAPI
builder.Services.AddReRouteSwagger();

// Banco de dados Oracle
// A connection string vem de variável de ambiente (OracleConnection__ConnectionString)
var connectionString = builder.Configuration["OracleConnection:ConnectionString"];
if (string.IsNullOrWhiteSpace(connectionString))
    throw new InvalidOperationException("OracleConnection:ConnectionString não configurada.");

builder.Services.AddDbContext<ReRouteDbContext>(options =>
    options.UseOracle(connectionString));

// Repositórios e serviços (injeção de dependência)
builder.Services.AddScoped<IParticipanteRepository, ParticipanteRepository>();
builder.Services.AddScoped<IDoacaoRepository, DoacaoRepository>();
builder.Services.AddScoped<ISolicitacaoRepository, SolicitacaoRepository>();
builder.Services.AddScoped<IEntregaRepository, EntregaRepository>();
builder.Services.AddScoped<IRegistroESGRepository, RegistroESGRepository>();

builder.Services.AddScoped<IParticipanteService, ParticipanteService>();
builder.Services.AddScoped<IDoacaoService, DoacaoService>();
builder.Services.AddScoped<ISolicitacaoService, SolicitacaoService>();
builder.Services.AddScoped<IEntregaService, EntregaService>();
builder.Services.AddScoped<IRegistroESGService, RegistroESGService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// Autenticação JWT
// A chave vem de variável de ambiente (Jwt__SecretKey), mínimo de 32 caracteres
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"];
if (string.IsNullOrWhiteSpace(jwtSecretKey))
    throw new InvalidOperationException("Jwt:SecretKey não configurado.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});

builder.Services.AddAuthorization();

// Health check usado pelo pipeline para verificar o deploy
builder.Services.AddHealthChecks();

var app = builder.Build();

// Middleware global de exceções (antes do pipeline MVC)
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReRoute ESG API v1");
        //options.RoutePrefix = string.Empty; // Swagger na raiz: http://localhost:5xxx/
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

// Aplica migrations e popula dados iniciais (exceto durante testes xUnit)
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ReRouteDbContext>();
    await SeedData.InitializeAsync(db);
}

app.Run();

// Necessário para testes de integração com WebApplicationFactory
public partial class Program { }
