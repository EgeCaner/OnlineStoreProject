using System;

namespace OnlineStoreProject.Models
{
    public class Discount
    {
        double discountRate {get; set;}
        int ProductId {get; set;}
        bool IsActive {get; set;} = true;
        public Nullable<DateTime> StartDate { get;  set; }
        public Nullable<DateTime> EndDate { get;  set; }
    }
}