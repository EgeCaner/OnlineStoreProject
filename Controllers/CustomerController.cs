using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using System.Linq;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreProject.Response;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;
using OnlineStoreProject_Intf;
using Microsoft.AspNetCore.Authorization;
using OnlineStoreProject.Services.CustomerService;

namespace OnlineStoreProject.Controllers.CustomerController
{   
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase{
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService){

            _customerService = customerService;
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() {
            ServiceResponse<List<CustomerDTO>> response = await _customerService.GetAllCustomers();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(){
            ServiceResponse<CustomerDTO> response =await _customerService.GetCustomerById();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(){
            ServiceResponse<string> response =await _customerService.DeleteUser();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateUser(CustomerDTO request){
            ServiceResponse<CustomerDTO> response = await _customerService.UpdateUser(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [HttpGet("GetRole")]
        public async Task<IActionResult> GetRole(){
            ServiceResponse<string> response = await _customerService.GetRole();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
