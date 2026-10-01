namespace ReRoute.Api.Models;

/// <summary>
/// Representa um participante do ecossistema ReRoute (doador, receptor ou transportador).
/// Mapeia a tabela Oracle ESG_PARTICIPANTE definida no projeto Java de referência.
/// </summary>
public class Participante
{
    public long ParticipantId { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    /// <summary>Senha em texto - apenas para protótipo acadêmico.</summary>
    /// <remarks>
    /// TODO (Kenny - Segurança):
    /// Motivo: senha em texto plano não é aceitável em API com autenticação.
    /// Arquivo: Participante.cs - aplicar hash; ParticipanteResponseDto não deve expor Senha.
    /// Resultado esperado: senha hasheada no banco; DTO de resposta sem campo Senha.
    /// </remarks>
    public string Senha { get; set; } = string.Empty;

    /// <summary>Valores esperados: DOADOR, RECEPTOR, TRANSPORTADOR.</summary>
    public string TipoPerfil { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public ICollection<Doacao> Doacoes { get; set; } = new List<Doacao>();

    public ICollection<Solicitacao> Solicitacoes { get; set; } = new List<Solicitacao>();

    public ICollection<Entrega> Entregas { get; set; } = new List<Entrega>();
}
