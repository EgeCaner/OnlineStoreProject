using System;

namespace OnlineStoreProject.Models
{
    public class Comment
    {   
        public int commentId {get; set;}
        public int ProductId {get; set;}
        public string CommentorName{get;set;}
        public string Description{get; set;}//Comment from the customer
        public  Nullable<System.DateTime> Date{get; set;}
        public Nullable<int> Like{get; set;} = 0;
        public bool isApproved {get; set;} = false;
    }
}