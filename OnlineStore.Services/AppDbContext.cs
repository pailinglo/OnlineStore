using System;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OnlineStore.Services
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        //inject dependency:
        public AppDbContext(DbContextOptions<AppDbContext> options) :
            base(options)
        { }
        
        //to make AppDbContext code clean, move Seed code to ModelBuilderExtensions.cs
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //to prevent error "The entity type 'IdentityUserLogin<string>' requires a primary key to be defined." 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
