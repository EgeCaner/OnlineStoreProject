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
        }
    }
}