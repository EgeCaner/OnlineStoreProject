namespace OnlineStoreProject.Models
{
    public class Comment
    {
        public int ProductId {get; set;}
        public string CommentorName{get;set;}
        public string Description{get; set;}//Comment from the customer
        public System.DateTime Date{get; set;}
        public int Like{get; set;}
    }
}