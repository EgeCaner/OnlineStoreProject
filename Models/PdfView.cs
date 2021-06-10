using OnlineStoreProject.Models;
using System.Collections.Generic;

namespace OnlineStoreProject.Models
{
    public class PdfView
    {
        public Customer customer {get; set;}
        public List<Order> order {get; set;}
        public List<Product> product {get; set;}
    }
}