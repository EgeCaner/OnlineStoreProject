using System;
using System.Collections.Generic;
using OnlineStoreProject.DTOs;

namespace OnlineStoreProject.DTOs
{
    public class ShoppingCartDTO
    {
        public int Id {get; set;}
        public List<CartItemDTO> Items {get; set;}
        public decimal TotalPrice {get; set;}
    
    }
}