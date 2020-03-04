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

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
