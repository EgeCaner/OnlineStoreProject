using System;
using System.Collections.Generic;
using OnlineStoreProject.Models;

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
        public string Address {get; set;} = null;
        public int TaxID {get; set;}= -1;
        public List<Comment> Comments{get; set;} = null;

    }
}