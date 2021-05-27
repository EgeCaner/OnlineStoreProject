using System.Threading.Tasks;
using OnlineStoreProject.Response;
using OnlineStoreProject.DTOs;
using System.Collections.Generic;

namespace OnlineStoreProject_Intf
{
    public interface IMailService
    {
        Task<ServiceResponse<string>> RegisterMail(string username);
        Task<ServiceResponse<string>> SendInvoice(List<OrderDTO> orders);
        Task<ServiceResponse<string>> ProductInTransit(int userId, int orderId);
        Task<ServiceResponse<string>> ProductDelivered(int userId, int orderId);
        Task<ServiceResponse<string>> ProductCancelled(int userId, int orderId);
        Task<ServiceResponse<string>> ProductRefunded(int userId, int orderId);
    }
}