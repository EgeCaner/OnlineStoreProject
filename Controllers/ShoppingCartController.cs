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
using OnlineStoreProject.Services;

namespace OnlineStoreProject.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet("GetCart")]
        public async Task<IActionResult> GetCartById(){
            ServiceResponse<ShoppingCartDTO> response = await _shoppingCartService.GetCartById();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("Add")]
        public async Task<IActionResult> AddCart(ShoppingCartDTO request){
            ServiceResponse<string> response = await _shoppingCartService.AddCart(request);
            if (!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        } 

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(){
            ServiceResponse<string> response = await _shoppingCartService.DeleteById();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ShoppingCartDTO request){
            ServiceResponse<ShoppingCartDTO> response = await _shoppingCartService.UpdateCart(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}