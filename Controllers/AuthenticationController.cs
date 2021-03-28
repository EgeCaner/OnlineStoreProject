using OnlineStoreProject_Intf.IAuthenticationService;
using System.Threading.Tasks;
using OnlineStoreProject.Request.UserLoginRequest;
using OnlineStoreProject.Request.UserRegisterRequest;
using OnlineStoreProject.Response.AuthenticationResponse;
using OnlineStoreProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStoreProject.Controllers.AuthenticationController
{   [ApiController]
    [Route("[controller]")]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            AuthenticationResponse<string> response = await _authService.Register(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            AuthenticationResponse<string> response = await _authService.Login(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        
    }
}