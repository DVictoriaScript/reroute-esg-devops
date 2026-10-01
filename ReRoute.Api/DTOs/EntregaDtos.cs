using System.ComponentModel.DataAnnotations;

namespace ReRoute.Api.DTOs;

public class EntregaCreateDto
{
    [Required]
    public long SolicitacaoId { get; set; }

    public long? ParticipantId { get; set; }

    public DateTime? DataAceite { get; set; }

    public DateTime? DataEntrega { get; set; }

    [MaxLength(20)]
    public string? Status { get; set; }
}

public class EntregaUpdateDto : EntregaCreateDto
{
    [Required]
    public long EntregaId { get; set; }
}

public class EntregaResponseDto
{
    public long EntregaId { get; set; }
    public long SolicitacaoId { get; set; }
    public long? ParticipantId { get; set; }
    public DateTime? DataAceite { get; set; }
    public DateTime? DataEntrega { get; set; }
    public string? Status { get; set; }
}
