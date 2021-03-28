namespace OnlineStoreProject.Request.UserRegisterRequest
{
    public class UserRegisterRequest
    {
        public string Name{get; set;} = null;
        public string Surname{get; set;} = null;
        public string Username{get; set;} = null;
        public string PhoneNumber{get; set;} = null;
        public string MailAddress { get; set; } = null;
        public string Password { get; set;} = null; 
    }
}