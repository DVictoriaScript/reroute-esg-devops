namespace ReRoute.Api.Models;

/// <summary>
/// Solicitação de um receptor interessado em uma doação específica.
/// Mapeia a tabela Oracle ESG_SOLICITACAO.
/// </summary>
public class Solicitacao
{
    public long SolicitacaoId { get; set; }

    public long DonationId { get; set; }

    public Doacao? Doacao { get; set; }

    public long ParticipantId { get; set; }

    public Participante? Participante { get; set; }

    public DateTime DataSolicitacao { get; set; }

    /// <summary>Ex.: PENDENTE, APROVADA, REJEITADA.</summary>
    public string? Status { get; set; }

    public ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();
}
