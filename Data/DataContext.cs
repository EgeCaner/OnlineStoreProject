using OnlineStoreProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Pomelo.EntityFrameworkCore.MySql;
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
    }
    
}