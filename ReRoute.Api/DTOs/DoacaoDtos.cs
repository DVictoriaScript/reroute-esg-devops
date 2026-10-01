using System.ComponentModel.DataAnnotations;

namespace ReRoute.Api.DTOs;

public class DoacaoCreateDto
{
    [Required]
    public long ParticipantId { get; set; }

    [Required]
    [MaxLength(100)]
    public string NomeItem { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Tamanho { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string LocalRetirada { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Status { get; set; }
}

public class DoacaoUpdateDto : DoacaoCreateDto
{
    [Required]
    public long DonationId { get; set; }
}

public class DoacaoResponseDto
{
    public long DonationId { get; set; }
    public long ParticipantId { get; set; }
    public string NomeItem { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Tamanho { get; set; } = string.Empty;
    public string LocalRetirada { get; set; } = string.Empty;
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
