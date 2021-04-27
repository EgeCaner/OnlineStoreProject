using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using System.Linq;
using OnlineStoreProject.Response;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStoreProject_Intf;
using OnlineStoreProject.Data.DataContext;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;

namespace OnlineStoreProject.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context=context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        public async Task<ServiceResponse<List<ProductDTO>>> GetAllProducts()
        {
            ServiceResponse<List<ProductDTO>> response = new ServiceResponse<List<ProductDTO>>();
            try{
            List<Product> dbProducts = await _context.Products.ToListAsync();
            response.Data= (dbProducts.Select(c => _mapper.Map<ProductDTO>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;

            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> AddProduct(ProductDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Product product = _mapper.Map<Product>(request);
                product.CreateDate = DateTime.Now;
                product.ModifyDate = DateTime.Now;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.PRODUCT_ADD_SUCCESS;
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
        
        public async Task<ServiceResponse<ProductDTO>> GetProductById(int Id){
            ServiceResponse<ProductDTO> response = new ServiceResponse<ProductDTO>();
            try{
            List<Comment> comment = await _context.Comments.Where(c => c.ProductId == Id).ToListAsync();
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == Id);
            response.Data =  _mapper.Map<ProductDTO>(dbProduct);
            response.Data.Comments = comment;
            response.Message = "Ok";
            response.Success=true;
            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteProductById(int Id){
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Product product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == Id);
                if (product != null)
                {
                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message = MessageConstants.PRODUCT_DELETE_SUCCESS;
                }
                else
                {
                    response.Success = false;
                    response.Message = MessageConstants.PRODUCT_NOT_FOUND;
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ProductDTO>> UpdateProduct(ProductDTO request){
            ServiceResponse<ProductDTO> response = new ServiceResponse<ProductDTO>();
            try{
                Product product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == request.ProductId);
                if (product ==null){
                    response.Success = false;
                    response.Message = MessageConstants.PRODUCT_UPDATE_FAIL;
                    return response;
                }
                product.ProductName = request.ProductName;
                product.CategoryId = request.CategoryId;
                product.Description = request.Description;
                product.ModifyDate = DateTime.Now;
                product.Price = request.Price;
                product.Quantity = request.Quantity;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.PRODUCT_UPDATE_SUCCESS;


            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

          public async Task<ServiceResponse<List<ProductDTO>>> GetProductByCategory(int Id)
        {
            ServiceResponse<List<ProductDTO>> response = new ServiceResponse<List<ProductDTO>>();
            try{
            List<Product> dbProducts = await _context.Products.Where(c=>c.CategoryId == Id).ToListAsync();
            response.Data= (dbProducts.Select(c => _mapper.Map<ProductDTO>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;

            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<List<int>>> GetAllCategories()
            {
            ServiceResponse<List<int>> response = new ServiceResponse<List<int>>();
            try{
            List<int> categories = await _context.Products.Select((c => c.CategoryId)).ToListAsync();
            response.Data= categories.Distinct().ToList();
            response.Message ="Ok";
            response.Success = true;

            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }


    
         public async Task<ServiceResponse<string>> AddComment(Comment request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Comment comment = _mapper.Map<Comment>(request);
                comment.Date = DateTime.Now;
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.COMMENT_ADD_SUCCESS;
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

         public async Task<ServiceResponse<Comment>> UpdateComment(Comment request){
            ServiceResponse<Comment> response = new ServiceResponse<Comment>();
            try{
                Comment comment = await _context.Comments.FirstOrDefaultAsync(c => c.commentId == request.commentId);
                if (comment == null){
                    response.Success = false;
                    response.Message = MessageConstants.COMMENT_UPDATE_FAIL;
                    return response;
                }

                comment.Description = request.Description;
                comment.Date = DateTime.Now;

                _context.Comments.Update(comment);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.COMMENT_UPDATE_SUCCESS;


            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }


        public async Task<ServiceResponse<List<Comment>>> GetAllComments(int Id)
        {
            ServiceResponse<List<Comment>> response = new ServiceResponse<List<Comment>>();
            try{
            List<Comment> comment = await _context.Comments.Where(c => c.ProductId == Id).ToListAsync();
            response.Data= (comment.Select(c => _mapper.Map<Comment>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;

            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteCommentById(int Id){
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Comment comment = await _context.Comments.FirstOrDefaultAsync(c => c.commentId == Id);
                if (comment != null)
                {
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message = MessageConstants.COMMENT_DELETE_SUCCESS;
                }
                else
                {
                    response.Success = false;
                    response.Message = MessageConstants.COMMENT_DELETE_FAIL;
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }


    }
}