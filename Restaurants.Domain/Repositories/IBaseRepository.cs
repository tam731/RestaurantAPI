namespace Restaurants.Domain.Repositories;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    IQueryable<TEntity> GetAll();
    Task<int> SaveChangeAsync();
}