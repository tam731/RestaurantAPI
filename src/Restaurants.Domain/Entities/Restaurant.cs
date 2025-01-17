﻿using Restaurants.Domain.Common;
using Restaurants.Domain.Entities.Identity;

namespace Restaurants.Domain.Entities
{
    public class Restaurant: BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Category { get; set; } = default!;
        public bool HasDelivery { get; set; }

        public string? ContactEmail { get; set; }
        public string? ContactNumber { get; set; }

        public Address? Address { get; set; }
        public List<Dish> Dishes { get; set; } = [];

        public User Owner { get; set; }=default!;
        public string OwnerId { get; set; } = default!;
    }
}