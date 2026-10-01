namespace ReRoute.Api.Middleware;

/// <summary>
/// Exceção de domínio para recursos não encontrados (HTTP 404).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message)
    {
    }
}
