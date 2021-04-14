using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;

namespace OnlineStoreProject_Intf
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDTO>>> GetAllProducts();
        Task<ServiceResponse<string>> AddProduct(ProductDTO request);
        Task<ServiceResponse<ProductDTO>> GetProductById(int Id);
        Task<ServiceResponse<string>> DeleteProductById(int Id);
        Task<ServiceResponse<ProductDTO>> UpdateProduct(ProductDTO request);
    }
}