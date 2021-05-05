using System;
using System.Collections.Generic;
using OnlineStoreProject.DTOs;
using OnlineStoreProject.Models;

namespace OnlineStoreProject.Models
{
    public class CartItem
    {
        public int Id {get; set;}        
        public int ProductId {get; set;}
        public Nullable<int> Quantity{get; set;}=0;
        public Nullable<System.DateTime> CreateDate{get; set;}
        public Nullable<System.DateTime> ModifyDate{get; set;}
        public Nullable<bool> IsActive{get; set;} = true;
        public Nullable<bool> IsDelete {get; set;} = false;
        public ShoppingCart ShoppingCart {get; set;}

    }
}