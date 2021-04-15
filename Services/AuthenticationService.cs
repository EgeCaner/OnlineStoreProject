using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnlineStoreProject_Intf.IAuthenticationService;
using OnlineStoreProject.Response.AuthenticationResponse;
using OnlineStoreProject.Request.UserLoginRequest;
using OnlineStoreProject.Request.UserRegisterRequest;
using OnlineStoreProject.Models;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;
using OnlineStoreProject.Data.DataContext;
using OnlineStoreProject.Request.ChangePasswordRequest;
using Microsoft.AspNetCore.Http;

namespace OnlineStoreProject.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration= configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<AuthenticationResponse<string>> Register(UserRegisterRequest request)
        {
             AuthenticationResponse<string> response = new AuthenticationResponse<string>();
            try{
                if (await UserExists(request.Username))
                {
                    response.Success = false;
                    response.Message = MessageConstants.USER_EXIST;  
                    return response;
                }
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                Customer user = new Customer{Name=request.Name,Surname= request.Surname,Username= request.Username, 
                MailAddress=request.MailAddress,PhoneNumber= request.PhoneNumber, PasswordHash = passwordHash,PasswordSalt=passwordSalt};

                await _context.Customers.AddAsync(user);
                await _context.SaveChangesAsync();
               //response.Data = user.Id;
                response.Success=true;
                response.Message= MessageConstants.USER_REGISTERED;
            }catch(Exception e){
                response.Success= false;
                response.Message =MessageConstants.USER_REGISTER_FAIL;
                response.Data = e.Message;
            }
            return response;
        }
        public async Task<AuthenticationResponse<string>> Login(UserLoginRequest request)
        {
            AuthenticationResponse<string> response = new AuthenticationResponse<string>();
            try{
                Customer customer = await _context.Customers.FirstOrDefaultAsync( c => c.Username.ToLower().Equals(request.Username.ToLower()));
                if(customer == null){
                    response.Success= false;
                    response.Message = MessageConstants.USER_WRONG_PASS_NAME_ERROR;
                }
                else if (!VerifyPasswordHash(request.Password,customer.PasswordHash,customer.PasswordSalt)){
                    response.Success= false;
                    response.Message = MessageConstants.USER_WRONG_PASS_NAME_ERROR;
                }
                else{
                    response.Success= true;
                    response.Message=MessageConstants.USER_LOGIN_SUCCESS;
                    response.Data = GenerateToken(customer);
                }
            }catch(Exception e){
                response.Success = false;
                response.Message=e.Message;
            }
            return response;
        }
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Customers.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }
         public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private string GenerateToken(Customer user){

           List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(2),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<AuthenticationResponse<string>> ChangePassword(ChangePasswordRequest request){
            AuthenticationResponse<string> response = new AuthenticationResponse<string>();
            try{
                Customer customer = await _context.Customers.FirstOrDefaultAsync( c => c.Username.ToLower().Equals(request.Username.ToLower()));
                if(customer == null){
                    response.Success= false;
                    response.Message = MessageConstants.USER_NOT_FOUND;
                }
                else if (!VerifyPasswordHash(request.Password,customer.PasswordHash,customer.PasswordSalt)){
                    response.Success= false;
                    response.Message = MessageConstants.USER_WRONG_PASS_NAME_ERROR;
                }
                else{
                    CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
                    customer.PasswordHash= passwordHash;
                    customer.PasswordSalt = passwordSalt;
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    response.Success = true;
                    response.Message =MessageConstants.PASSWORD_CHANGE_SUCCESS;
                    response.Data = GenerateToken(customer);
                }
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }

    }
}