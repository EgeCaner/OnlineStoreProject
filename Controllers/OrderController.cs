using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Response;
using OnlineStoreProject_Intf;
using onlinestoreproject_be.Models;

namespace onlinestoreproject_be.Controllers
{
        [Authorize]
        [ApiController]
        [Route("[controller]")]
        public class OrderController : ControllerBase
        {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService){

            _orderService = orderService; 
        }
        
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllOrders() {
            ServiceResponse<List<OrderDTO>> response = await _orderService.GetAllOrders();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("AddOrder")]
         public async Task<IActionResult> AddOrder(OrderDTO request){
            ServiceResponse<string> response = await _orderService.AddOrder(request);
            if (!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        } 


    }
}