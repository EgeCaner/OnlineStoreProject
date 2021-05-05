using System;
namespace OnlineStoreProject.Models
{
    public class Product
    {
        public int ProductId{get; set;}
        public string ProductName{get; set;} = null;
        public int CategoryId{get; set;}
        public string Description{get; set;} = null;
        public string ImageUrl{get; set;} = null;
        public Nullable<System.DateTime> CreateDate{get; set;}
        public Nullable<System.DateTime> ModifyDate{get; set;}
        public Nullable<int> Quantity{get; set;} = 0;
        public Nullable<bool> IsActive{get; set;} = false;
        public Nullable<bool> IsDelete {get; set;} = false;
        public Nullable<double> Rating{get; set;} = null;
        public Nullable<decimal> Price{get; set;} = null;
        public string WarrantyStatus {get; set;} =null;
    }
}