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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService){

            _productService = productService; 
        }

        [HttpGet("GetAll")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll() {
            ServiceResponse<List<ProductDTO>> response = await _productService.GetAllProducts();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Add")]
        [AllowAnonymous]
        public async Task<IActionResult> AddProduct(ProductDTO request){
            ServiceResponse<string> response = await _productService.AddProduct(request);
            if (!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        } 

        [HttpGet("GetById/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int Id){
            ServiceResponse<ProductDTO> response = await _productService.GetProductById(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int Id){
            ServiceResponse<string> response = await _productService.DeleteProductById(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductDTO request){
            ServiceResponse<ProductDTO> response = await _productService.UpdateProduct(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpGet("GetByCategory/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByCategory(int id){
             ServiceResponse<List<ProductDTO>> response = await _productService.GetProductByCategory(id);
             if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpGet("GetAllCategories")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories() {
            ServiceResponse<List<int>> response = await _productService.GetAllCategories();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpPut("AddComment")]
        public async Task<IActionResult> AddComment(Comment request){
            ServiceResponse<string> response = await _productService.AddComment(request);
            if (!response.Success){
                return BadRequest(response);
            }
            return Ok(response);
        } 

        [HttpPut("UpdateComment")]
        public async Task<IActionResult> UpdateComment(Comment request){
            ServiceResponse<Comment> response = await _productService.UpdateComment(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetAllComments/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllComments(int Id) {
            ServiceResponse<List<Comment>> response = await _productService.GetAllComments(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeleteComment/{id}")]
        public async Task<IActionResult> DeleteComment(int Id){
            ServiceResponse<string> response = await _productService.DeleteCommentById(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("ApproveComment/{id}")]
        public async Task<IActionResult> ApproveComment(int Id){
            ServiceResponse<string> response = await _productService.ApproveComment(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("RejectComment/{id}")]
        public async Task<IActionResult> RejectComment(int Id){
            ServiceResponse<string> response = await _productService.RejectComment(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet("GetUnApprovedComments")]
        public async Task<IActionResult> GetUnApprovedComments(){
            ServiceResponse<List<Comment>> response = await _productService.GetUnApprovedComments();
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("DeletePermanent/{id}")]
        public async Task<IActionResult> DeletePermanent(int Id){
            ServiceResponse<string> response = await _productService.DeleteProduct(Id);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}