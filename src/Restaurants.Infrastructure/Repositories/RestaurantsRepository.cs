using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories
{
    internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
    {
        public async Task<int> CreateAsync(Restaurant entity)
        {
           dbContext.Restaurants.Add(entity);
           await dbContext.SaveChangesAsync();
            return entity.Id;
        }

        public async Task DeleteAsync(Restaurant entity)
        {
            dbContext.Restaurants.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetAllAsync()
        {
            var restaurants = await dbContext.Restaurants.ToListAsync();
            return restaurants;
        }
        public async Task<(IEnumerable<Restaurant>,int)> GetAllMatchingAsync(string? searchPhrase,
                                                                             int pageSize,
                                                                             int pageNumber, 
                                                                             string? sortBy,
                                                                             SortDirection sortDirection)
        {
            string searchPhraseLower = (searchPhrase??"").ToLower();

            var baseQuery = dbContext.Restaurants.Where(r => string.IsNullOrWhiteSpace(searchPhrase)
                                                                        || (r.Name.ToLower().Contains(searchPhraseLower)
                                                                          || r.Description.ToLower().Contains(searchPhraseLower)));

            if (sortBy != null)
            {
                var columnSelector=new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                    {nameof(Restaurant.Name),r=>r.Name},
                    {nameof(Restaurant.Category),r=>r.Category},
                    {nameof(Restaurant.Description),r=>r.Description},
                };

                if (columnSelector.ContainsKey(sortBy))
                {
                    var selectedColumn = columnSelector[sortBy];

                    baseQuery=sortDirection== SortDirection.Ascending
                        ?baseQuery.OrderBy(selectedColumn)
                        :baseQuery.OrderByDescending(selectedColumn);
                }
                
            }

            var totalCount=await baseQuery.CountAsync();

            var restaurants = await baseQuery.Skip(pageSize*(pageNumber-1))
                                             .Take(pageSize)
                                             .ToListAsync();
            return (restaurants, totalCount);
        }

        public async Task<Restaurant?> GetByIdAsync(int id)
        {
            var restaurant = await dbContext.Restaurants.Include(r=>r.Dishes).FirstOrDefaultAsync(x => x.Id == id);
            return restaurant;
        }



        public Task SaveChangeAsync()=> dbContext.SaveChangesAsync();
        
        
    }
}
