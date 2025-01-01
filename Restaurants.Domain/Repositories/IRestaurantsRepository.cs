using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<int> CreateAsync(Restaurant entity);

        Task DeleteAsync(Restaurant entity);

        Task<IEnumerable<Restaurant>> GetAllAsync();

        Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);

        Task<Restaurant?> GetByIdAsync(int id);

        Task SaveChangeAsync();
    }
}