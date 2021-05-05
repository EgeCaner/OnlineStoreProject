using System;
using System.Collections.Generic;
using OnlineStoreProject.DTOs;

namespace OnlineStoreProject.Models
{
    public class ShoppingCart
    {
        public int Id {get; set;}
        public int UserId {get; set;}
        public Nullable<System.DateTime> CreateDate{get; set;}
        public Nullable<System.DateTime> ModifyDate{get; set;}
        public Nullable<bool> IsActive{get; set;} = true;
        public Nullable<bool> IsDelete {get; set;} = false;

    }
}