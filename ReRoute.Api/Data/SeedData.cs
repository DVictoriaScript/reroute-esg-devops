using Microsoft.EntityFrameworkCore;
using ReRoute.Api.Models;

namespace ReRoute.Api.Data;

/// <summary>
/// Dados iniciais para desenvolvimento e demonstração acadêmica.
/// Espelha o seed do InMemoryDataStoreSpring do projeto Java.
/// </summary>
/// <remarks>
/// TODO (Paula - Banco):
/// Motivo: seed automático pode conflitar com dados reais do Oracle FIAP.
/// Arquivo: SeedData.cs - desativar ou adaptar seed após integração com Oracle.
/// Resultado esperado: banco acadêmico populado sem duplicar registros em produção.
/// </remarks>
public static class SeedData
{
    public static async Task InitializeAsync(ReRouteDbContext context)
    {
        if (context.Database.IsRelational())
        {
            await context.Database.MigrateAsync();
        }

        if (await context.Participantes.AnyAsync())
            return;

        var participantes = new List<Participante>
        {
            new() { Nome = "João Silva", Email = "joao@email.com", Senha = "senha1", TipoPerfil = "DOADOR", CreatedAt = new DateTime(2024, 1, 1, 10, 0, 0) },
            new() { Nome = "Maria Souza", Email = "maria@email.com", Senha = "senha2", TipoPerfil = "RECEPTOR", CreatedAt = new DateTime(2024, 2, 1, 11, 0, 0) },
            new() { Nome = "Carlos Lima", Email = "carlos@email.com", Senha = "senha3", TipoPerfil = "TRANSPORTADOR", CreatedAt = new DateTime(2024, 3, 1, 12, 0, 0) },
            new() { Nome = "Ana Paula", Email = "ana@email.com", Senha = "senha4", TipoPerfil = "DOADOR", CreatedAt = new DateTime(2024, 4, 1, 13, 0, 0) },
            new() { Nome = "Lucas Rocha", Email = "lucas@email.com", Senha = "senha5", TipoPerfil = "RECEPTOR", CreatedAt = new DateTime(2024, 5, 1, 14, 0, 0) }
        };

        context.Participantes.AddRange(participantes);
        await context.SaveChangesAsync();

        var doacoes = new List<Doacao>
        {
            new() { ParticipantId = participantes[0].ParticipantId, NomeItem = "Cesta Básica", Descricao = "Cesta com alimentos variados", Tamanho = "MEDIO", LocalRetirada = "Rua A, 123", Status = "DISPONIVEL", CreatedAt = new DateTime(2024, 1, 10, 9, 0, 0) },
            new() { ParticipantId = participantes[1].ParticipantId, NomeItem = "Roupas", Descricao = "Roupas de inverno", Tamanho = "GRANDE", LocalRetirada = "Rua B, 456", Status = "SOLICITADA", CreatedAt = new DateTime(2024, 2, 15, 10, 0, 0) },
            new() { ParticipantId = participantes[2].ParticipantId, NomeItem = "Brinquedos", Descricao = "Brinquedos educativos", Tamanho = "PEQUENO", LocalRetirada = "Rua C, 789", Status = "CONCLUIDA", CreatedAt = new DateTime(2024, 3, 20, 11, 0, 0) },
            new() { ParticipantId = participantes[3].ParticipantId, NomeItem = "Livros", Descricao = "Livros infantis", Tamanho = "MEDIO", LocalRetirada = "Rua D, 101", Status = "DISPONIVEL", CreatedAt = new DateTime(2024, 4, 25, 12, 0, 0) },
            new() { ParticipantId = participantes[4].ParticipantId, NomeItem = "Material Escolar", Descricao = "Kit escolar completo", Tamanho = "GRANDE", LocalRetirada = "Rua E, 202", Status = "SOLICITADA", CreatedAt = new DateTime(2024, 5, 30, 13, 0, 0) }
        };

        context.Doacoes.AddRange(doacoes);
        await context.SaveChangesAsync();

        var solicitacoes = new List<Solicitacao>
        {
            new() { DonationId = doacoes[0].DonationId, ParticipantId = participantes[3].ParticipantId, DataSolicitacao = new DateTime(2024, 6, 1, 14, 0, 0), Status = "PENDENTE" },
            new() { DonationId = doacoes[2].DonationId, ParticipantId = participantes[4].ParticipantId, DataSolicitacao = new DateTime(2024, 7, 2, 15, 0, 0), Status = "APROVADA" },
            new() { DonationId = doacoes[4].DonationId, ParticipantId = participantes[0].ParticipantId, DataSolicitacao = new DateTime(2024, 8, 3, 16, 0, 0), Status = "REJEITADA" },
            new() { DonationId = doacoes[1].DonationId, ParticipantId = participantes[2].ParticipantId, DataSolicitacao = new DateTime(2024, 9, 4, 17, 0, 0), Status = "PENDENTE" },
            new() { DonationId = doacoes[3].DonationId, ParticipantId = participantes[1].ParticipantId, DataSolicitacao = new DateTime(2024, 10, 5, 18, 0, 0), Status = "APROVADA" }
        };

        context.Solicitacoes.AddRange(solicitacoes);
        await context.SaveChangesAsync();

        var entregas = new List<Entrega>
        {
            new() { SolicitacaoId = solicitacoes[0].SolicitacaoId, ParticipantId = participantes[1].ParticipantId, DataAceite = new DateTime(2024, 6, 1, 14, 0, 0), DataEntrega = new DateTime(2024, 6, 5, 10, 0, 0), Status = "PENDENTE" },
            new() { SolicitacaoId = solicitacoes[1].SolicitacaoId, ParticipantId = participantes[3].ParticipantId, DataAceite = new DateTime(2024, 7, 2, 15, 0, 0), DataEntrega = new DateTime(2024, 7, 6, 11, 0, 0), Status = "EM_TRANSITO" },
            new() { SolicitacaoId = solicitacoes[2].SolicitacaoId, ParticipantId = participantes[0].ParticipantId, DataAceite = new DateTime(2024, 8, 3, 16, 0, 0), DataEntrega = new DateTime(2024, 8, 7, 12, 0, 0), Status = "ENTREGUE" },
            new() { SolicitacaoId = solicitacoes[3].SolicitacaoId, ParticipantId = participantes[2].ParticipantId, DataAceite = new DateTime(2024, 9, 4, 17, 0, 0), DataEntrega = new DateTime(2024, 9, 8, 13, 0, 0), Status = "PENDENTE" },
            new() { SolicitacaoId = solicitacoes[4].SolicitacaoId, ParticipantId = participantes[4].ParticipantId, DataAceite = new DateTime(2024, 10, 5, 18, 0, 0), DataEntrega = new DateTime(2024, 10, 9, 14, 0, 0), Status = "EM_TRANSITO" }
        };

        context.Entregas.AddRange(entregas);
        await context.SaveChangesAsync();

        var registros = new List<RegistroESG>
        {
            new() { DonationId = doacoes[0].DonationId, ResiduoEvitado = 10.50m, Co2Evitado = 5.20m, DataRegistro = new DateTime(2024, 6, 1, 14, 0, 0) },
            new() { DonationId = doacoes[1].DonationId, ResiduoEvitado = 20.00m, Co2Evitado = 10.00m, DataRegistro = new DateTime(2024, 7, 2, 15, 0, 0) },
            new() { DonationId = doacoes[2].DonationId, ResiduoEvitado = 15.00m, Co2Evitado = 7.50m, DataRegistro = new DateTime(2024, 8, 3, 16, 0, 0) },
            new() { DonationId = doacoes[3].DonationId, ResiduoEvitado = 25.00m, Co2Evitado = 12.50m, DataRegistro = new DateTime(2024, 9, 4, 17, 0, 0) },
            new() { DonationId = doacoes[4].DonationId, ResiduoEvitado = 30.00m, Co2Evitado = 15.00m, DataRegistro = new DateTime(2024, 10, 5, 18, 0, 0) }
        };

        context.RegistrosESG.AddRange(registros);
        await context.SaveChangesAsync();
    }
}
