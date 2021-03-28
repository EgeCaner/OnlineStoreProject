namespace OnlineStoreProject.Response.AuthenticationResponse
{
    public class AuthenticationResponse<T>
    {
        public T Data {get; set;}
        public bool Success {get; set;} =false;
        public string Message {get; set;}= null;
    }
}