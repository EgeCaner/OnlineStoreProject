using System.Linq;
using AutoMapper;
using OnlineStoreProject.Models;
using OnlineStoreProject.DTOs;
using onlinestoreproject_be.Models;

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
        }
    }
}