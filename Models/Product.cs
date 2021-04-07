using System;
namespace OnlineStoreProject.Models
{
    public class Product
    {
        public int ProductId{get; set;}
        public string ProductName{get; set;}
        public int CategoryId{get; set;}
        public string Description{get; set;}
        public string ImageUrl{get; set;}
        public Nullable<System.DateTime> CreateDate{get; set;}
        public Nullable<System.DateTime> ModifyDate{get; set;}
        public Nullable<int> Quantity{get; set;}
        public Nullable<bool> IsActive{get; set;}
        public Nullable<bool> IsDelete {get; set;}
        public Nullable<double> Rating{get; set;}
        public Nullable<decimal> Price{get; set;}
    }
}