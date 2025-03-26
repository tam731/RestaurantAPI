namespace Restaurants.Infrastructure.Configuration;

public class RedisConfiguration
{
    public bool Enabled { get; set; }
    public string ConnectionString { get; set; } = default!;
}