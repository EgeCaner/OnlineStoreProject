using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;

namespace OnlineStoreProject_Intf
{
    public interface ICustomerService
    {
        Task<ServiceResponse<List<CustomerDTO>>> GetAllCustomers();
        Task<ServiceResponse<CustomerDTO>> GetCustomerById();
        Task<ServiceResponse<string>> DeleteUser();
        Task<ServiceResponse<CustomerDTO>> UpdateUser(CustomerDTO request);
        Task<ServiceResponse<string>> GetRole();
    }
}