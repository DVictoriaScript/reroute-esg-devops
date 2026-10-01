using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReRoute.Api.Data;

namespace ReRoute.Api.Tests;

public class ReRouteApiFactory : WebApplicationFactory<Program>
{
    // Cada factory usa seu próprio banco em memória para as classes de teste não interferirem entre si
    private readonly string _nomeBanco = "ReRouteTestsDb-" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Valores só para teste: o banco real é trocado pelo InMemory logo abaixo
        builder.UseSetting("OracleConnection:ConnectionString", "Data Source=localhost:1521/TESTE");
        builder.UseSetting("Jwt:SecretKey", "chave-de-teste-reroute-com-mais-de-32-caracteres");
        builder.UseSetting("Jwt:Issuer", "ReRoute.Api");
        builder.UseSetting("Jwt:Audience", "ReRoute.Client");

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ReRouteDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<ReRouteDbContext>(options =>
                options.UseInMemoryDatabase(_nomeBanco));

            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ReRouteDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedData.InitializeAsync(db).GetAwaiter().GetResult();
        });
    }
}