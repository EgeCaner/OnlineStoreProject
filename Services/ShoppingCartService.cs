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
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context=context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<string>> AddCart(ShoppingCartDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                ShoppingCart cart = new ShoppingCart();
                cart.UserId = GetUserId();
                cart.CreateDate = DateTime.Now;
                cart.ModifyDate = DateTime.Now;
                await _context.ShoppingCarts.AddAsync(cart);
                await _context.SaveChangesAsync();
                ShoppingCart carts = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == GetUserId() && c.IsDelete ==false);

                for (int i =0; i < request.Items.Count; i++)
                {
                    CartItem cartItems = new CartItem();
                    cartItems.CreateDate = DateTime.Now;
                    cartItems.ProductId = request.Items[i].ProductId;
                    int? quantity = request.Items[i].Quantity;
                    cartItems.Quantity = quantity;
                    cartItems.ShoppingCart = carts;
                    await _context.CartItems.AddAsync(cartItems);
                }
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.PRODUCT_ADD_SUCCESS;// change the message
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
    
        public async Task<ServiceResponse<string>> DeleteById()
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                ShoppingCart cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == GetUserId());
                if (cart != null)
                {    List<CartItem> carts =  await _context.CartItems.Where(c=> c.ShoppingCart.Id == cart.Id).ToListAsync();
                    if(cart != null){
                        for (int i = 0; i < carts.Count; i++)
                        {
                            _context.CartItems.Remove(carts[i]);
                        }
                    }
                    _context.ShoppingCarts.Remove(cart);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message = MessageConstants.PRODUCT_DELETE_SUCCESS;//Change Message
                }
                else
                {
                    response.Success = false;
                    response.Message = MessageConstants.PRODUCT_NOT_FOUND; //Change Message
                }

            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<ShoppingCartDTO>> GetCartById()
        {
            ServiceResponse<ShoppingCartDTO> response = new ServiceResponse<ShoppingCartDTO>();
            try{
            ShoppingCart dbCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == GetUserId());

                if(dbCart !=null){
                List<CartItem> cartItems = await _context.CartItems.Where(c => c.ShoppingCart.Id ==dbCart.Id).ToListAsync();
                    if(cartItems !=null){
                        int totalPrice = 0;
                        List<ProductDTO> products = new List<ProductDTO>();
                        for(int i= 0; i<cartItems.Count; i++){
                            Product product= await _context.Products.FirstOrDefaultAsync(c => c.ProductId == cartItems[i].ProductId);
                            ProductDTO prod = _mapper.Map<ProductDTO>(product);
                            if(prod != null){
                                products.Add(prod);
                                //totalPrice += product.Price* (decimal?)(int)cartItems[i].Quantity; 
                            }
                        }
                        ShoppingCartDTO newCart = new ShoppingCartDTO();
                        newCart.Id = dbCart.Id;
                        newCart.Items=products;
                        newCart.TotalPrice = totalPrice;
                        response.Data = newCart;
                        response.Success=true;
                        response.Message= "Ok";
                    }
                }
                else{
                    response.Message = "User does not have a cart!";
                    response.Success = false;
                }
            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }
        public async Task<ServiceResponse<ShoppingCartDTO>> UpdateCart(ShoppingCartDTO request)
        {
            ServiceResponse<ShoppingCartDTO> response = new ServiceResponse<ShoppingCartDTO>();
            try{
                ShoppingCart cart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == GetUserId());
                if (cart ==null){
                    response.Success = false;
                    response.Message = MessageConstants.PRODUCT_UPDATE_FAIL;//change the message
                    return response;
                }
                cart.ModifyDate = DateTime.Now;
                _context.ShoppingCarts.Update(cart);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.PRODUCT_UPDATE_SUCCESS;//change the message


            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }
    }
}