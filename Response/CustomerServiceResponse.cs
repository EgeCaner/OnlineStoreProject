namespace OnlineStoreProject.Response
{
    public class CustomerServiceResponse<T>
    {
        public T Data {get; set;}
        public bool Success {get; set;} =false;
        public string Message {get; set;}= null;
    }
}