using AutoMapper;
using DataAccessLayer.AccessLayer.Models;
using ProductManagement.DTOs;

namespace ProductManagement.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
