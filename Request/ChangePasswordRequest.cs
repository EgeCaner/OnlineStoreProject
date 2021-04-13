namespace OnlineStoreProject.Request.ChangePasswordRequest
{
    public class ChangePasswordRequest
    {
        public string Username {get; set;} = null;
        public string Password {get; set;} = null;
        public string NewPassword {get; set;} = null;
    }
}