
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

using onlinestoreproject_be.Models;
using OnlineStoreProject_Intf;

namespace OnlineStoreProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<OrderDTO>>> GetAllOrders()
        {
            ServiceResponse<List<OrderDTO>> response = new ServiceResponse<List<OrderDTO>>();
            try{
            List<Order> dbOrders = await _context.Orders.ToListAsync();
            response.Data= (dbOrders.Select(c => _mapper.Map<OrderDTO>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;

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
                }
        
           
                Order Order = _mapper.Map<Order>(request);
                Order.CreateDate = DateTime.Now;

                await _context.Orders.AddAsync(Order);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.ORDER_ADD_SUCCES;
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
                    response.Message = MessageConstants.ORDER_NOT_FOUND;//Change the message
                    return response;
                }

                order.Status = request.Status;
                order.Quantity = request.Quantity;
                order.Price= request.Price;
                order.CreateDate = DateTime.Now;
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.ORDER_UPDATE_SUCCES;//change to order message


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
                    dbOrder.Status = request.Status;
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
    }
}