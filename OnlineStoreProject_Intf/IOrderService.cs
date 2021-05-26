using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;
using System;



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
        Task<ServiceResponse<string>> Refund(int orderId);
        Task<ServiceResponse<List<OrderDTO>>> GetPendingRefunds();
        Task<ServiceResponse<List<decimal>>> SalesAnalytics(DateTime start, DateTime end);
    }
}