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
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using System.IO;
using Wkhtmltopdf.NetCore;
using System.Text;
using InvoicerNETCore.Services;
using InvoicerNETCore.Services.Impl;
using InvoicerNETCore.Models;
using InvoicerNETCore.Helpers;
namespace OnlineStoreProject.Services
{
    public class MailService : IMailService
    {
        
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IGeneratePdf _generatePdf;
        public MailService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor, IGeneratePdf generatePdf)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _generatePdf = generatePdf;
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

        public async Task<ServiceResponse<string>> SendInvoice(int Id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            try{
            Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == Id);
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("TechIst 34","techist9@gmail.com"));
            message.To.Add(new MailboxAddress(dbCustomer.Name + " " + dbCustomer.Surname,dbCustomer.MailAddress));
            message.Subject =  "Invoice";
            var body = new TextPart("plain"){
                Text =  "Dear " + dbCustomer.Name 
                + ",\nYou have succesfuly purchased products. Invoice of your purchase is attached to this mail. \nFor any problem you can reach us from: techist9@gmail.com" 
            };
            string path = "./21235.pdf";
            var bodyBuilder = new BodyBuilder();
            var attachment = (MimePart) bodyBuilder.Attachments.Add(path);
            attachment.ContentTransferEncoding = ContentEncoding.Base64;
           
            var multipart = new Multipart ("mixed");
            multipart.Add(body);
            multipart.Add(attachment);


            message.Body = multipart;

            using (var client = new SmtpClient()){
                client.Connect("smtp.gmail.com", 587 ,false);
                client.Authenticate("techist9@gmail.com", "techist@34");
                client.Send(message);
                client.Disconnect(true);
            }
            response.Success = true;
            response.Message= "Ok";
            System.Diagnostics.Debug.WriteLine("Sending invoice mail success");
            }catch(Exception e){
                response.Success = false;
                response.Message = e.Message;
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            return response;
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

        public async Task<ServiceResponse<string>> CreatePdf(int Id){
            ServiceResponse<string> response = new ServiceResponse<string>();
            
            try{
                Customer dbCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == Id);
                List<Order> dbOrder = await _context.Orders.Where(c => c.CustomerId == dbCustomer.Id &&  c.CreateDate.Day == DateTime.Now.Day).ToListAsync();
                List<Product> prods = new List<Product>();
                foreach (var order in dbOrder)
                {
                    Product dbProduct = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == order.ProductId);
                    prods.Add(dbProduct);
                }
                decimal sum=0;
                foreach (var order in dbOrder)
                {

                    sum+=order.Price;
                }

                Guid rand = Guid.NewGuid();
                List<ItemRow> itr = new List<ItemRow> ();
                 for(int i=0;i<prods.Count;i++)
                    {
                        itr.Add(ItemRow.Make(prods[i].ProductName, prods[i].Description, (int)dbOrder[i].Quantity, 20,(decimal)prods[i].Price, (decimal)dbOrder[i].Price));
                    }
                new InvoicerApi(SizeOption.A4, OrientationOption.Landscape, "$")
                .TextColor("#CC0000")
                .BackColor("#FFD6CC")
                .Company(Address.Make("FROM", new string [] {"TechIst Limited", "TechIst House"}))
                .Client(Address.Make("BILLING TO",new string [] {dbCustomer.Name+" "+dbCustomer.Surname, dbCustomer.Address , "WR## 2DJ"}))
                .Items(itr)
                .Totals(new List<TotalRow> {
                    TotalRow.Make("Total", (decimal)sum, true)
                })
                .Details(new List<DetailRow> {
                    DetailRow.Make("PAYMENT INFORMATION", "Make all cheques payable to TechIst TR Limited.", "", "If you have any questions concerning this invoice, contact our sales department at techist9@gmail.com.", "", "Thank you for your business.")
                })
                .Footer("http://www.TechIst.co.tr")
                .Save();
    
                if(dbOrder != null){
                  response.Success = true;
                  response.Message = "Ok";
                }
                var ans =await SendInvoice(Id);
            }catch(Exception e){
                response.Success= false;
                response.Message= e.Message;
                System.Diagnostics.Debug.WriteLine("pdf exception");
            }
            return response;
        }
    }
}