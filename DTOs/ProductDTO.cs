using System;
using OnlineStoreProject.Models;
using System.Collections.Generic;


namespace onlinestoreproject_be.DTOs
{
    public class ProductDTO
    {
        public int ProductId{get; set;} 
        public string ProductName{get; set;}= null;
        public string Description{get; set;}= null;
        public string ImageUrl{get; set;}= null;
        public Nullable<int> Quantity{get; set;}= null;
        public Nullable<double> Rating{get; set;}= null;
        public Nullable<decimal> Price{get; set;}= null;        
        public List<Comment> Comments{get; set;} = null;
    }
}