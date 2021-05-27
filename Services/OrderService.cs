
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStoreProject.Data.DataContext;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Models;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;
using OnlineStoreProject.Response;
using OnlineStoreProject_Intf;

namespace OnlineStoreProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;

        public OrderService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<OrderDTO>>> GetAllOrders()
        {
            ServiceResponse<List<OrderDTO>> response = new ServiceResponse<List<OrderDTO>>();
            try{
            List<Order> dbOrders = await _context.Orders.ToListAsync();
            if(dbOrders != null){
            response.Data= (dbOrders.Select(c => _mapper.Map<OrderDTO>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;
            }else{
                response.Success = false;
                response.Message = MessageConstants.ORDER_NOT_FOUND;
            }
            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> AddOrder(OrderDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Product product = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == request.ProductId);
                if(request.Quantity > 0 && ((product.Quantity - request.Quantity) >= 0) ){
                product.Quantity = product.Quantity - request.Quantity;
                _context.Products.Update(product);
                Order Order = _mapper.Map<Order>(request);
                Order.CreateDate = DateTime.Now;
                Order.CustomerId = GetUserId();
                await _context.Orders.AddAsync(Order);
                ShoppingCart dbCart = await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == GetUserId());
                if (dbCart !=null){
                CartItem dbCartItem = await _context.CartItems.FirstOrDefaultAsync(c => c.ShoppingCart == dbCart && c.ProductId == request.ProductId);
                _context.CartItems.Remove(dbCartItem);
                }
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.ORDER_ADD_SUCCES;
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_PRODUCT_QUANTITY_INEFFICIENT;
                }

            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
            public async Task<ServiceResponse<OrderDTO>> UpdateOrder(OrderDTO request){
            ServiceResponse<OrderDTO> response = new ServiceResponse<OrderDTO>();
            try{
                Order order = await _context.Orders.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (order == null){
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND;
                    return response;
                }
                
                if(order.Status == 0 && request.Status ==3){
                    order.Status =3;
                    Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == request.ProductId);
                    dbProduct.Quantity += request.Quantity;
                    order.Status = request.Status;
                    _context.Products.Update(dbProduct); 
                    await _mailService.ProductCancelled(GetUserId(), order.Id);
                }else if(request.Status ==3){
                        response.Success = false;
                        response.Message = MessageConstants.ORDER_CANCEL_FAIL;
                        return response;
                }else{
                    order.Status = request.Status;
                }
                
                order.Quantity = request.Quantity;
                order.Price= request.Price;
                order.ModifyDate = DateTime.Now;
                order.Address = request.Address;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.ORDER_UPDATE_SUCCES;


            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetOrdersByUserId()
        {
            ServiceResponse<List<OrderDTO>> response  = new ServiceResponse<List<OrderDTO>>();
            try{
                List<Order> dbOrders = await _context.Orders.Where(c => c.CustomerId == GetUserId()).ToListAsync();
                if(dbOrders != null){
                    response.Data = (dbOrders.Select(c => _mapper.Map<OrderDTO>(c))).ToList();
                    response.Message ="Ok";
                    response.Success = true;
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND;
                }
            }catch(Exception e){
                response.Message = e.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> DeleteOrderById(int Id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Order dbOrder = await _context.Orders.FirstOrDefaultAsync(c => c.Id == Id);
                if (dbOrder != null){
                    _context.Orders.Remove(dbOrder);
                    await _context.SaveChangesAsync();
                    response.Message ="Ok";
                    response.Success = true;
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND;
                }
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ChangeOrderStatus(OrderDTO request)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Order dbOrder = await _context.Orders.FirstOrDefaultAsync(c => c.Id == request.Id);
                if (dbOrder != null){
                    if (request.Status == 5){
                        Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == request.ProductId);
                        dbProduct.Quantity += request.Quantity;
                        dbOrder.Status = request.Status;
                        _context.Products.Update(dbProduct); 
                        await _mailService.ProductRefunded(GetUserId(), dbOrder.Id);
                    }else if(request.Status == 1){
                        dbOrder.Status = request.Status;
                        await _mailService.ProductInTransit(GetUserId(), dbOrder.Id);
                    }else if(request.Status == 2){
                        dbOrder.Status = request.Status;
                        await _mailService.ProductDelivered(GetUserId(), dbOrder.Id);
                    }else{
                        dbOrder.Status = request.Status;
                    }
                    _context.Orders.Update(dbOrder);
                    await _context.SaveChangesAsync();
                    response.Message= MessageConstants.ORDER_UPDATE_SUCCES;
                    response.Success = true;
                }else{
                    response.Success =  false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND;
                }
            }catch(Exception e){
                response.Message = e.Message;
                response.Success = false;
            }
            return response;
        }

        public async Task<ServiceResponse<string>> Refund(int orderId){
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Order order = await _context.Orders.FirstOrDefaultAsync(c => c.Id == orderId && c.CustomerId == GetUserId());
                if (order == null){
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND;
                    return response;
                }

                order.Status = 4;
                order.ModifyDate = DateTime.Now;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.ORDER_UPDATE_SUCCES;


            }catch(Exception e){
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<OrderDTO>>> GetPendingRefunds(){
            ServiceResponse<List<OrderDTO>> response = new ServiceResponse<List<OrderDTO>>();
            try{
                
                List<Order> dbOrders = await _context.Orders.Where(c => c.Status == 4).ToListAsync();
                if (dbOrders != null){
                    response.Success = true;
                    response.Data = (dbOrders.Select(c => _mapper.Map<OrderDTO>(c))).ToList();
                    response.Message = "Ok";
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.ORDER_NOT_FOUND; 
                }
            }catch(Exception e){
                response.Message = e.Message;
                response.Success = false;
            }
            return response;
        } 

        public async Task<ServiceResponse<List<decimal>>> SalesAnalytics(DateTime start, DateTime end){
            ServiceResponse<List<decimal>> response = new ServiceResponse<List<decimal>>();
            if(false){
                List<Order> dbOrders = await _context.Orders.Where(c => c.Status != 3 && c.Status !=5).ToListAsync();

            }
            response.Success =false;
            response.Message = MessageConstants.METHOD_NOT_IMPLEMENTED;
            return response;
        }
    }
}