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
{   [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase{
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService){

            _customerService = customerService;
        }
        [AllowAnonymous] // use this except for ordering buyin etc.. the conditions that user need to login
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll() {
            return Ok(await _customerService.GetAllCustomers());

        }
   }
}
