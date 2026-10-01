namespace ReRoute.Api.ViewModels;

/// <summary>
/// Parâmetros de paginação recebidos via query string nos endpoints de listagem.
/// Exemplo: GET /api/doacoes?page=1&amp;pageSize=10
/// </summary>
public class PaginationQuery
{
    private const int MaxPageSize = 100;

    private int _page = 1;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? 10 : value);
    }
}
