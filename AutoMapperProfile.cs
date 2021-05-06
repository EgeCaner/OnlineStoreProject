using System.Linq;
using AutoMapper;
using OnlineStoreProject.Models;
using OnlineStoreProject.DTOs;

namespace OnlineStoreProject
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile(){
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO,Customer>();
            CreateMap<Product,ProductDTO>();
            CreateMap<ProductDTO, Product>();
            CreateMap<OrderDTO, Order>();
            CreateMap<Order, OrderDTO>();
            CreateMap<ShoppingCart,ShoppingCartDTO>();
            CreateMap<ShoppingCartDTO,ShoppingCart>();
            CreateMap<Product,CartItemDTO>();
        }
    }
}