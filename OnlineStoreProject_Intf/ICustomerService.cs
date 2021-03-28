using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;


namespace OnlineStoreProject_Intf
{
    public interface ICustomerService
    {
        Task<CustomerServiceResponse<List<CustomerDTO>>> GetAllCustomers();
    }
}