using Microsoft.EntityFrameworkCore;
using ReRoute.Api.Data;
using ReRoute.Api.ViewModels;

namespace ReRoute.Api.Repositories;

/// <summary>
/// Implementação base de repositório usando EF Core.
/// </summary>
public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly ReRouteDbContext Context;
    protected readonly DbSet<TEntity> DbSet;

    public Repository(ReRouteDbContext context)
    {
        Context = context;
        DbSet = context.Set<TEntity>();
    }

    public virtual async Task<TEntity?> GetByIdAsync(long id) =>
        await DbSet.FindAsync(id);

    public virtual async Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize)
    {
        var query = DbSet.AsNoTracking();
        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResult<TEntity>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = total,
            TotalPages = (int)Math.Ceiling(total / (double)pageSize)
        };
    }

    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(long id) =>
        await DbSet.FindAsync(id) is not null;
}
