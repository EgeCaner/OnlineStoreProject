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
    public DataContext()
    {
    }
     //public virtual Microsoft.EntityFrameworkCore.DbContextOptionsBuilder UseLoggerFactory (Microsoft.Extensions.Logging.ILoggerFactory loggerFactory);
     //public partial void LoggerFactory (System.Collections.Generic.IEnumerable<Microsoft.Extensions.Logging.ILoggerProvider> providers, Microsoft.Extensions.Logging.LoggerFilterOptions filterOptions);
     public DataContext(DbContextOptions<DataContext> options): base(options){}
     public virtual DbSet <Customer> Customers {get; set;}
     public virtual DbSet <Product> Products {get; set;}
     public DbSet <Order> Orders {get; set;}
     public virtual DbSet <Comment> Comments {get; set;}
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
    protected override void OnModelCreating(ModelBuilder modelBuilder){
        Utility.CreatePasswordHash("12345",out byte[] passwordHash, out byte[] passwordSalt);
        modelBuilder.Entity<Customer>().HasData(
            new Customer {Id = 11,  Username= "seco",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 12,  Username= "ege",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 13,  Username= "ahmet",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 14,  Username= "zeynep",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 15,  Username= "baris",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 16,  Username= "ilayda",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 17,  Username= "merve",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 18,  Username= "gulce",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 19,  Username= "yusuf",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 20,  Username= "volkan",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 21,  Username= "Mehmet",PasswordHash= passwordHash, PasswordSalt = passwordSalt },
            new Customer {Id = 22,  Username= "Ismail",PasswordHash= passwordHash, PasswordSalt = passwordSalt }


        );
            
        //modelBuilder.Entity<Comment>()
            //..Property(Comment => Comment.).HasDefaultValue()
    }

    }
    
}