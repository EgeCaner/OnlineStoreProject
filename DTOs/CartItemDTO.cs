using System;
using System.Collections.Generic;
using OnlineStoreProject.Models;

namespace OnlineStoreProject.DTOs
{
    public class CartItemDTO
    {
        public int CartItemId {get; set;}
        public int ProductId{get; set;}
        public int CategoryId{get; set;}
        public string ProductName{get; set;}= null;
        public string Description{get; set;}= null;
        public string ImageUrl{get; set;}= null;
        public Nullable<int> Quantity{get; set;}= null;
        public Nullable<double> Rating{get; set;}= null;
        public Nullable<decimal> Price{get; set;}= null;        
        public List<Comment> Comments{get; set;} = null;
        public decimal DiscountRate {get; set;} = 0;
        public string WarrantyStatus {get; set;} = "Two years of warranty";
        public decimal DiscountedPrice {get; set;}
    }
}