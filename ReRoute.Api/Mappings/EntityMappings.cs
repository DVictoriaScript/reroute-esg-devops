using ReRoute.Api.DTOs;
using ReRoute.Api.Models;

namespace ReRoute.Api.Mappings;

/// <summary>
/// Conversões entre Models (banco) e DTOs/ViewModels (API).
/// Mantém controllers enxutos e facilita avaliação acadêmica da separação de camadas.
/// </summary>
public static class EntityMappings
{
    public static ParticipanteResponseDto ToResponse(this Participante entity) => new()
    {
        ParticipantId = entity.ParticipantId,
        Nome = entity.Nome,
        Email = entity.Email,
        TipoPerfil = entity.TipoPerfil,
        CreatedAt = entity.CreatedAt
    };

    public static Participante ToEntity(this ParticipanteCreateDto dto) => new()
    {
        Nome = dto.Nome,
        Email = dto.Email,
        Senha = dto.Senha,
        TipoPerfil = dto.TipoPerfil,
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyUpdate(this Participante entity, ParticipanteUpdateDto dto)
    {
        entity.Nome = dto.Nome;
        entity.Email = dto.Email;
        entity.Senha = dto.Senha;
        entity.TipoPerfil = dto.TipoPerfil;
    }

    public static DoacaoResponseDto ToResponse(this Doacao entity) => new()
    {
        DonationId = entity.DonationId,
        ParticipantId = entity.ParticipantId,
        NomeItem = entity.NomeItem,
        Descricao = entity.Descricao,
        Tamanho = entity.Tamanho,
        LocalRetirada = entity.LocalRetirada,
        Status = entity.Status,
        CreatedAt = entity.CreatedAt
    };

    public static Doacao ToEntity(this DoacaoCreateDto dto) => new()
    {
        ParticipantId = dto.ParticipantId,
        NomeItem = dto.NomeItem,
        Descricao = dto.Descricao,
        Tamanho = dto.Tamanho,
        LocalRetirada = dto.LocalRetirada,
        Status = dto.Status ?? "DISPONIVEL",
        CreatedAt = DateTime.UtcNow
    };

    public static void ApplyUpdate(this Doacao entity, DoacaoUpdateDto dto)
    {
        entity.ParticipantId = dto.ParticipantId;
        entity.NomeItem = dto.NomeItem;
        entity.Descricao = dto.Descricao;
        entity.Tamanho = dto.Tamanho;
        entity.LocalRetirada = dto.LocalRetirada;
        entity.Status = dto.Status;
    }

    public static SolicitacaoResponseDto ToResponse(this Solicitacao entity) => new()
    {
        SolicitacaoId = entity.SolicitacaoId,
        DonationId = entity.DonationId,
        ParticipantId = entity.ParticipantId,
        DataSolicitacao = entity.DataSolicitacao,
        Status = entity.Status
    };

    public static Solicitacao ToEntity(this SolicitacaoCreateDto dto) => new()
    {
        DonationId = dto.DonationId,
        ParticipantId = dto.ParticipantId,
        DataSolicitacao = DateTime.UtcNow,
        Status = dto.Status ?? "PENDENTE"
    };

    public static void ApplyUpdate(this Solicitacao entity, SolicitacaoUpdateDto dto)
    {
        entity.DonationId = dto.DonationId;
        entity.ParticipantId = dto.ParticipantId;
        entity.Status = dto.Status;
    }

    public static EntregaResponseDto ToResponse(this Entrega entity) => new()
    {
        EntregaId = entity.EntregaId,
        SolicitacaoId = entity.SolicitacaoId,
        ParticipantId = entity.ParticipantId,
        DataAceite = entity.DataAceite,
        DataEntrega = entity.DataEntrega,
        Status = entity.Status
    };

    public static Entrega ToEntity(this EntregaCreateDto dto) => new()
    {
        SolicitacaoId = dto.SolicitacaoId,
        ParticipantId = dto.ParticipantId,
        DataAceite = dto.DataAceite,
        DataEntrega = dto.DataEntrega,
        Status = dto.Status ?? "PENDENTE"
    };

    public static void ApplyUpdate(this Entrega entity, EntregaUpdateDto dto)
    {
        entity.SolicitacaoId = dto.SolicitacaoId;
        entity.ParticipantId = dto.ParticipantId;
        entity.DataAceite = dto.DataAceite;
        entity.DataEntrega = dto.DataEntrega;
        entity.Status = dto.Status;
    }

    public static RegistroESGResponseDto ToResponse(this RegistroESG entity) => new()
    {
        RegistroId = entity.RegistroId,
        DonationId = entity.DonationId,
        ResiduoEvitado = entity.ResiduoEvitado,
        Co2Evitado = entity.Co2Evitado,
        DataRegistro = entity.DataRegistro
    };

    public static RegistroESG ToEntity(this RegistroESGCreateDto dto) => new()
    {
        DonationId = dto.DonationId,
        ResiduoEvitado = dto.ResiduoEvitado,
        Co2Evitado = dto.Co2Evitado,
        DataRegistro = DateTime.UtcNow
    };

    public static void ApplyUpdate(this RegistroESG entity, RegistroESGUpdateDto dto)
    {
        entity.DonationId = dto.DonationId;
        entity.ResiduoEvitado = dto.ResiduoEvitado;
        entity.Co2Evitado = dto.Co2Evitado;
    }
}
