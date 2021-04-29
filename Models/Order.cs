using System;

namespace onlinestoreproject_be.Models
{
    public class Order
    {

        public int Id{get; set;}

        public int Quantity{get; set;}

        public int ProductId{get;set;}
        public int CustomerId{get; set;}

        public int Status{get; set;}
    
        public Nullable<DateTime> CreateDate { get;  set; }
    }
}