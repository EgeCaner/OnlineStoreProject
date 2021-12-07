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
using OnlineStoreProject_Intf;
using OnlineStoreProject.Response.AuthenticationResponse;
using OnlineStoreProject.Request.UserLoginRequest;
using OnlineStoreProject.Request.UserRegisterRequest;
using OnlineStoreProject.Models;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;
using OnlineStoreProject.Data.DataContext;
using OnlineStoreProject.Request.ChangePasswordRequest;
using Microsoft.AspNetCore.Http;
using OnlineStoreProject.Services;

namespace OnlineStoreProject.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMailService _mailService;

        public AuthenticationService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMailService mailService)
        {
            _configuration= configuration;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mailService = mailService;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<AuthenticationResponse<string>> Register(UserRegisterRequest request)
        {
            AuthenticationResponse<string> response = new AuthenticationResponse<string>();
            try{
                if(request.Username!= null && request.Password !=null){
                    if (await UserExists(request.Username))
                    {
                        response.Success = false;
                        response.Message = MessageConstants.USER_EXIST;  
                        return response;
                    }
                    Utility.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    Customer user = new Customer{Name=request.Name,Surname= request.Surname,Username= request.Username, 
                    MailAddress=request.MailAddress,PhoneNumber= request.PhoneNumber, PasswordHash = passwordHash,PasswordSalt=passwordSalt};

                    await _context.Customers.AddAsync(user);
                    await _context.SaveChangesAsync();
                    Customer dbCustomer =  await _context.Customers.FirstOrDefaultAsync(c => c.Username == request.Username);
                    ShoppingCart dbCart = new ShoppingCart();        
                    dbCart.UserId = dbCustomer.Id;
                    dbCart.CreateDate = DateTime.Now;   
                    dbCart.ModifyDate = DateTime.Now;         
                    await _context.ShoppingCarts.AddAsync(dbCart);
                    await _context.SaveChangesAsync();
                    await _mailService.RegisterMail(request.Username);
                    response.Success=true;
                    response.Message= MessageConstants.USER_REGISTERED;
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.USERNAME_OR_PASSWORD_NULL;
                }
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
                if(request.Username != null &&  request.Password != null){
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
                }else{
                    response.Success = false;
                    response.Message = MessageConstants.USERNAME_OR_PASSWORD_NULL;
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
                new Claim(ClaimTypes.Role, user.Role)
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
                Customer customer = await _context.Customers.FirstOrDefaultAsync( c => c.Id == GetUserId());
                if(customer == null){
                    response.Success= false;
                    response.Message = MessageConstants.USER_NOT_FOUND;
                }
                else if (!VerifyPasswordHash(request.Password,customer.PasswordHash,customer.PasswordSalt)){
                    response.Success= false;
                    response.Message = MessageConstants.USER_WRONG_PASS_NAME_ERROR;
                }
                else{
                    Utility.CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
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