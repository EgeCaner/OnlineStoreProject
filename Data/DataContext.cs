using OnlineStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Pomelo.EntityFrameworkCore.MySql;
using onlinestoreproject_be.Models;
using onlinestoreproject_be.Services;

namespace OnlineStoreProject.Data.DataContext
{
    public class DataContext : DbContext
    {
     public DataContext(DbContextOptions<DataContext> options): base(options){}
     public virtual DbSet <Customer> Customers {get; set;}
     public virtual DbSet <Product> Products {get; set;}
     public  DbSet <Order> Orders {get; set;}
     public virtual DbSet <Comment> Comments {get; set;}
     public virtual DbSet <ShoppingCart> ShoppingCarts {get; set;}
     public virtual DbSet <CartItem> CartItems {get; set;}

     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
         optionsBuilder
                .UseMySql(   "server=remotemysql.com;port=3306;user=e9kvb80H9D;password=moKwFVzbog;database=e9kvb80H9D;",
            new MySqlServerVersion(new Version(8, 0, 11)))
                .UseLoggerFactory(LoggerFactory.Create(b => b
                .AddConsole()
                .AddFilter(level => level <= LogLevel.Information)))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
     }
    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        Utility.CreatePasswordHash("12345",out byte[] passwordHash, out byte[] passwordSalt);
        modelBuilder.Entity<Customer>().HasData(
            
        );
            
        //modelBuilder.Entity<Comment>()
            //..Property(Comment => Comment.).HasDefaultValue()
    }
    */
    }
}