using System.ComponentModel.DataAnnotations;

namespace ReRoute.Api.DTOs;

public class SolicitacaoCreateDto
{
    [Required]
    public long DonationId { get; set; }

    [Required]
    public long ParticipantId { get; set; }

    [MaxLength(20)]
    public string? Status { get; set; }
}

public class SolicitacaoUpdateDto : SolicitacaoCreateDto
{
    [Required]
    public long SolicitacaoId { get; set; }
}

public class SolicitacaoResponseDto
{
    public long SolicitacaoId { get; set; }
    public long DonationId { get; set; }
    public long ParticipantId { get; set; }
    public DateTime DataSolicitacao { get; set; }
    public string? Status { get; set; }
}
