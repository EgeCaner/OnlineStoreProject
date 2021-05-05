using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;
using OnlineStoreProject.Models;
namespace OnlineStoreProject_Intf
{
    public interface IShoppingCartService
    {
       Task<ServiceResponse<string>> AddCart(ShoppingCartDTO request);
        Task<ServiceResponse<ShoppingCartDTO>> GetCartById();
        Task<ServiceResponse<ShoppingCartDTO>> UpdateCart(ShoppingCartDTO request);
        Task<ServiceResponse<string>> DeleteById();
    }
}