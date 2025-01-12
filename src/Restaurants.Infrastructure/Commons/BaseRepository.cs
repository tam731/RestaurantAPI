using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Commons;

internal class BaseRepository<TEntity>(RestaurantsDbContext dbContext) : IBaseRepository<TEntity> where TEntity : class
{
    public async Task CreateAsync(TEntity entity)
    {
        await dbContext.AddAsync(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
       dbContext.Remove(entity);
        return Task.CompletedTask;
    }

    public IQueryable<TEntity> GetAll()
    =>dbContext.Set<TEntity>();

    public async Task<int> SaveChangeAsync()=>await dbContext.SaveChangesAsync();
  
}