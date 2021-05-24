using System;

namespace OnlineStoreProject.Models
{
    public class Order
    {   
        public int Id{get; set;}
        public int Quantity{get; set;}
        public int ProductId{get;set;}
        public int CustomerId{get; set;}
        public string Status{get; set;} = "Processing"; 
        public string Address {get; set;}
        public Nullable<DateTime> CreateDate { get;  set; }
        public decimal Price {get; set;}
        public Nullable<DateTime> ModifyDate { get;  set; }
    }
}