using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;



namespace OnlineStoreProject_Intf
{
    public interface IOrderService
    {  
        Task<ServiceResponse<List<OrderDTO>>> GetAllOrders();
        Task<ServiceResponse<string>> AddOrder(OrderDTO request);
        Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request);
        Task<ServiceResponse<List<OrderDTO>>> GetOrdersByUserId();
        Task<ServiceResponse<string>> DeleteOrderById(int Id);
        Task<ServiceResponse<string>> ChangeOrderStatus(OrderDTO request);
    }
}