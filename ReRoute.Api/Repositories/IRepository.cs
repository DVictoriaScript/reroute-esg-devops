using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Repositories;

/// <summary>
/// Contrato genérico de repositório - abstrai o Entity Framework dos Services.
/// </summary>
public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(long id);
    Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize);
    Task<TEntity> AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(long id);
}
