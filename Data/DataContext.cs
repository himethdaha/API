using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using System;
using API.Entities;

namespace API.Data
{
    public class DataContext : DbContext
    {
        //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        //    :base(options)
        //{ 
        //
        public DataContext(DbContextOptions options)
            : base(options)
        {

        }

        public virtual Microsoft.EntityFrameworkCore.DbSet<AppUser> Users { get; set; }
      


        
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
          
        //{
        //    optionsBuilder
        //        .UseMySql("server = localhost; port = 3306; database = Rocky; user = blog; password = blog")
        //        .UseLoggerFactory(LoggerFactory.Create(b => b
        //               .AddConsole()
        //               .AddFilter(level => level >= LogLevel.Information)))
        //        .EnableSensitiveDataLogging()
        //        .EnableDetailedErrors();
        //}
    }
}
