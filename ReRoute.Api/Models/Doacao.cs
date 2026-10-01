namespace ReRoute.Api.Models;

/// <summary>
/// Item doado disponível para solicitação no fluxo ReRoute.
/// Mapeia a tabela Oracle ESG_DOACAO.
/// </summary>
public class Doacao
{
    public long DonationId { get; set; }

    public long ParticipantId { get; set; }

    public Participante? Participante { get; set; }

    public string NomeItem { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Tamanho { get; set; } = string.Empty;

    public string LocalRetirada { get; set; } = string.Empty;

    /// <summary>Ex.: DISPONIVEL, SOLICITADA, CONCLUIDA.</summary>
    public string? Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();

    public ICollection<RegistroESG> RegistrosESG { get; set; } = new List<RegistroESG>();
}
