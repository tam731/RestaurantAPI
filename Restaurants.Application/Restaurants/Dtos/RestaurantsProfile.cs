﻿using AutoMapper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.DTOs
{
    public class RestaurantsProfile:Profile
    {
        public RestaurantsProfile()
        {
            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(d => d.City, opt =>
                        opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(d => d.PostalCode, opt =>
                        opt.MapFrom(src => src.Address == null ? null : src.Address.PostalCode))
                .ForMember(d => d.Street, opt =>
                        opt.MapFrom(src => src.Address == null ? null : src.Address.City))
                .ForMember(d => d.Dishes, opt =>
                        opt.MapFrom(src => src.Dishes));

            CreateMap<CreateRestaurantCommand, Restaurant>()
                .ForMember(d => d.Address, opt => opt.MapFrom(
                    src => new Address
                    {
                        City = src.City,
                        PostalCode = src.PostalCode,
                        Street = src.Street,
                    }));

        }
    }
}
