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

namespace OnlineStoreProject.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CustomerService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context=context;
        }
        
        public async Task<CustomerServiceResponse<List<CustomerDTO>>> GetAllCustomers(){
            CustomerServiceResponse<List<CustomerDTO>> response = new CustomerServiceResponse<List<CustomerDTO>>();
            try{
            List<Customer> dbCustomers = await _context.Customers.ToListAsync();
            response.Data= (dbCustomers.Select(c => _mapper.Map<CustomerDTO>(c))).ToList();
            response.Message ="Ok";
            response.Success = true;

            }catch(Exception e){
                response.Success= false;
                response.Message = e.Message;
                return response;
            }
            return response;
        }

    }
}