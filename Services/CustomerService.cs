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

namespace OnlineStoreProject.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context=context;
            _httpContextAccessor = httpContextAccessor;
        }
        
        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        private string GetUserRole() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        public async Task<ServiceResponse<List<CustomerDTO>>> GetAllCustomers(){
            ServiceResponse<List<CustomerDTO>> response = new ServiceResponse<List<CustomerDTO>>();
            try{
            List<Customer> dbCustomers = await _context.Customers.ToListAsync();
            if(dbCustomers !=null){
                response.Data= (dbCustomers.Select(c => _mapper.Map<CustomerDTO>(c))).ToList();
                response.Message ="Ok";
                response.Success = true;
            }else{
                response.Success= false;
                response.Message = MessageConstants.USER_NOT_FOUND;
            }
            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

        public async Task<ServiceResponse<CustomerDTO>> GetCustomerById(){
            ServiceResponse<CustomerDTO> response = new ServiceResponse<CustomerDTO>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == GetUserId());
            response.Data =  _mapper.Map<CustomerDTO>(dbCustomer);
            response.Message = "Ok";
            response.Success=true;
            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }
        
        public async Task<ServiceResponse<string>> DeleteUser(){
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Customer customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == GetUserId());
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message = MessageConstants.USER_DELETE_SUCCESS;
                }
                else
                {
                    response.Success = false;
                    response.Message = MessageConstants.USER_NOT_FOUND;
                }
            }
            catch(Exception e)
            {
                response.Success = false;
                response.Message= e.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<CustomerDTO>> UpdateUser(CustomerDTO request){
            ServiceResponse<CustomerDTO> response = new ServiceResponse<CustomerDTO>();
            try{
                Customer customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == GetUserId());
                if (customer ==null){
                    response.Success = false;
                    response.Message = MessageConstants.USER_UPDATE_FAIL;
                    return response;
                }
                customer.Username = request.Username;
                customer.Name = request.Name;
                customer.Surname = request.Surname;
                customer.MailAddress = request.MailAddress;
                customer.PhoneNumber = request.PhoneNumber;
                customer.Address = request.Address;
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = MessageConstants.USER_UPDATE_SUCCESS;
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
        public async Task<ServiceResponse<string>> GetRole(){
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
                Customer dbCust = await _context.Customers.FirstOrDefaultAsync(c => c.Id == GetUserId());
                if(dbCust != null){
                    response.Success=true;
                    response.Message= "Ok";
                    response.Data = dbCust.Role;
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.USER_NOT_FOUND;
                }
            }catch(Exception e){
                response.Message= e.Message;
                response.Success= false;
            }
            return response;
        }
    }
}