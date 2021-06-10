using System;

namespace OnlineStoreProject.Models
{
    public class Order
    {   
        public int Id{get; set;}
        public int Quantity{get; set;}
        public int ProductId{get;set;}
        public int CustomerId{get; set;}
        public int Status{get; set;} = 0; 
        public string Address {get; set;}
        public DateTime CreateDate { get;  set; }
        public decimal Price {get; set;}
        public Nullable<DateTime> ModifyDate { get;  set; }
    }
}