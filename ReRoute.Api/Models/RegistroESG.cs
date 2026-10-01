namespace ReRoute.Api.Models;

/// <summary>
/// Registro de impacto ambiental (resíduo evitado e CO₂) de uma doação concluída.
/// Mapeia a tabela Oracle ESG_REGISTRO.
/// </summary>
public class RegistroESG
{
    public long RegistroId { get; set; }

    public long DonationId { get; set; }

    public Doacao? Doacao { get; set; }

    public decimal ResiduoEvitado { get; set; }

    public decimal Co2Evitado { get; set; }

    public DateTime DataRegistro { get; set; }
}
