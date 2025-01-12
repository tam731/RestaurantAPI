using System.ComponentModel.DataAnnotations;

namespace Restaurants.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}