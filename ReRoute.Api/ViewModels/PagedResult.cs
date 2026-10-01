namespace ReRoute.Api.ViewModels;

/// <summary>
/// ViewModel genérico de resposta paginada para listagens da API.
/// Separa o formato de saída HTTP dos Models de persistência (padrão acadêmico MVVM na API).
/// </summary>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; set; } = Array.Empty<T>();

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }

    public bool HasPrevious => Page > 1;

    public bool HasNext => Page < TotalPages;
}
