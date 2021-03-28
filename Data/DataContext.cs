using OnlineStoreProject.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineStoreProject.Data.DataContext
{
    public class DataContext : DbContext
    {
     public DataContext(DbContextOptions<DataContext> options): base(options){}
     public DbSet <Customer> Customers {get; set;}
    }
}