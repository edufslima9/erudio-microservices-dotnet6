using AutoMapper;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Models;

namespace GeekShopping.CartAPI.Config
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMapps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductVO>().ReverseMap();
                config.CreateMap<Cart, CartVO>().ReverseMap();
                config.CreateMap<CartHeader, CartHeaderVO>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailVO>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
