namespace ReRoute.Api.Models;

/// <summary>
/// Entrega realizada por um transportador vinculada a uma solicitação.
/// Mapeia a tabela Oracle ESG_ENTREGA.
/// </summary>
public class Entrega
{
    public long EntregaId { get; set; }

    public long SolicitacaoId { get; set; }

    public Solicitacao? Solicitacao { get; set; }

    /// <summary>Transportador responsável - pode ser nulo no início do fluxo.</summary>
    public long? ParticipantId { get; set; }

    public Participante? Participante { get; set; }

    public DateTime? DataAceite { get; set; }

    public DateTime? DataEntrega { get; set; }

    /// <summary>Ex.: PENDENTE, EM_TRANSITO, ENTREGUE.</summary>
    public string? Status { get; set; }
}
