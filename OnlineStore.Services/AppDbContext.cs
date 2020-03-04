using System;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class AppDbContext : DbContext
    {
        //inject dependency:
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        { }
        
        //to make AppDbContext code clean, move Seed code to ModelBuilderExtensions.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
