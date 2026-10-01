using System.ComponentModel.DataAnnotations;

namespace ReRoute.Api.DTOs;

public class RegistroESGCreateDto
{
    [Required]
    public long DonationId { get; set; }

    [Range(0, 999.99)]
    public decimal ResiduoEvitado { get; set; }

    [Range(0, 999.99)]
    public decimal Co2Evitado { get; set; }
}

public class RegistroESGUpdateDto : RegistroESGCreateDto
{
    [Required]
    public long RegistroId { get; set; }
}

public class RegistroESGResponseDto
{
    public long RegistroId { get; set; }
    public long DonationId { get; set; }
    public decimal ResiduoEvitado { get; set; }
    public decimal Co2Evitado { get; set; }
    public DateTime DataRegistro { get; set; }
}
