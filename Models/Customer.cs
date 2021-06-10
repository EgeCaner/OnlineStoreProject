using System.ComponentModel.DataAnnotations;
namespace OnlineStoreProject.Models
{
    public class Customer
    {
        public int Id {get; set;}
        public string Name{get; set;} = null;
        public string Surname{get; set;} = null;
        public string Username{get; set;} = null;
        public string PhoneNumber{get; set;} = null;
        public string MailAddress { get; set; } = null;
        public byte[] PasswordHash { get; set;} = null;
        public byte[] PasswordSalt {get; set;} = null;    
        public int TaxId {get; set;} = -1;
        public ShoppingCart Cart {get; set;} = null;    
        [Required]
        public string Role {get; set;} =  "Customer";
        public string Address {get; set;} = "Unknown";
    }
}