using System;
using OnlineStoreProject.Models;
using System.Collections.Generic;
namespace OnlineStoreProject.DTOs
{
    public class OrderDTO
    {

        public int Id{get; set;}

        public int Quantity{get; set;}

        public int CustomerId{get; set;}

        public int Status{get; set;}
    
        public int ProductId{get;set;}
  


  
        public DateTime CreateDate { get; internal set; }
    
    }
}