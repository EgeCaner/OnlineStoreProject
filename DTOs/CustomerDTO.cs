namespace OnlineStoreProject.DTOs
{
    public class CustomerDTO
    {
        public int Id {get; set;}
        public string Name{get; set;} = null;
        public string Surname{get; set;} = null;
        public string Username{get; set;} = null;
        public string PhoneNumber{get; set;} = null;
        public string MailAddress { get; set; } = null;
    }
}