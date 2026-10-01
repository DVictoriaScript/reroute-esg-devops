using System.ComponentModel.DataAnnotations;

namespace ReRoute.Api.DTOs;

/// <summary>DTO de entrada para criação de participante.</summary>
public class ParticipanteCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(200)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [MinLength(4, ErrorMessage = "A senha deve ter ao menos 4 caracteres.")]
    [MaxLength(200)]
    public string Senha { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo de perfil é obrigatório.")]
    [RegularExpression("DOADOR|RECEPTOR|TRANSPORTADOR", ErrorMessage = "Perfil deve ser DOADOR, RECEPTOR ou TRANSPORTADOR.")]
    public string TipoPerfil { get; set; } = string.Empty;
}

public class ParticipanteUpdateDto : ParticipanteCreateDto
{
}

public class ParticipanteResponseDto
{
    public long ParticipantId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string TipoPerfil { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
