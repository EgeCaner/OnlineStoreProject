using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OnlineStoreProject.Data.DataContext;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Models;
using OnlineStoreProject.OnlineStoreConstants.MessageConstants;
using OnlineStoreProject.Response;
using OnlineStoreProject_Intf;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;


namespace OnlineStoreProject.Services
{
    public class MailService : IMailService
    {
        
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MailService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<string>> RegisterMail(string username)
        {   
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Username == username);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Registeration";
            message.Body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nWelcome to TechIst, you can securely buy tech products with best prices. \nFor any problem you can reach us from: techist9@gmail.com" 
            };

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending register mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
        }

        public Task<ServiceResponse<string>> SendInvoice(List<OrderDTO> orders)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<string>> ProductInTransit(int userId, int orderId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Order on the way";
            message.Body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nYour order with order ID:"+ orderId +", on the way and will be delivered in 2 to 5 days" 
            };

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending order in-transit mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ProductDelivered(int userId, int orderId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Order Delivered";
            message.Body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nYour order with order ID:"+ orderId +", is delivered. If any deformation or unsatisfaction occurs you can refund the product in 30-days" 
            };

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending order delivered mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ProductCancelled(int userId, int orderId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Order Cancelled";
            message.Body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nYour order with order ID:"+ orderId +", is cancelled and your purchase will be refunded at the last working day of the week" 
            };

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending order cancel mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
        }

        public async Task<ServiceResponse<string>> ProductRefunded(int userId, int orderId)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == userId);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Order Refund Success";
            message.Body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nYour refund demand is approved by the sales manager, your purchase will be refunded at the last working day of the week.\nTechIst team will be glad if you explain the reason why you returned the product to improve our quality and customer satisfaction\nReach us from: techist9@gmail.com" 
            };

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending Refund mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
        }
    }
}