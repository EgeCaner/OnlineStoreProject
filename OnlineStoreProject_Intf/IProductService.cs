using System.Threading.Tasks;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using System.Collections.Generic;
using OnlineStoreProject.Models;

namespace OnlineStoreProject_Intf
{
    public interface IProductService
    {
        Task<ServiceResponse<List<ProductDTO>>> GetAllProducts();
        Task<ServiceResponse<string>> AddProduct(ProductDTO request);
        Task<ServiceResponse<ProductDTO>> GetProductById(int Id);
        Task<ServiceResponse<string>> DeleteProductById(int Id);
        Task<ServiceResponse<ProductDTO>> UpdateProduct(ProductDTO request);
        Task<ServiceResponse<List<ProductDTO>>> GetProductByCategory(int Id);
        Task<ServiceResponse<string>> AddComment(Comment request);
        Task<ServiceResponse<Comment>> UpdateComment(Comment request);
        Task<ServiceResponse<List<int>>> GetAllCategories();
        Task<ServiceResponse<List<Comment>>> GetAllComments(int Id);
        Task<ServiceResponse<string>> DeleteCommentById(int Id);
        Task<ServiceResponse<string>> ApproveComment(int Id);
        Task<ServiceResponse<string>> RejectComment(int Id); 
        Task<ServiceResponse<List<Comment>>> GetUnApprovedComments();   
        Task<ServiceResponse<string>> DeleteProduct(int Id);   
        Task<ServiceResponse<string>> SetDiscount(decimal discRate, int prodId);
    }
}