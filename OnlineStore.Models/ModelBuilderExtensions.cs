using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    Name = "Herb",
                    Description = "Herb"
                },
                new Category
                {
                    CategoryId = 2,
                    Name = "Shrub",
                    Description = "Shrub"
                },
                new Category
                {
                    CategoryId = 3,
                    Name = "Bush",
                    Description = "Bush"
                },
                new Category
                {
                    CategoryId = 4,
                    Name = "Tree",
                    Description = "Tree"
                },
                new Category
                {
                    CategoryId = 5,
                    Name = "Grass",
                    Description = "Grass"
                },
                new Category
                {
                    CategoryId = 6,
                    Name = "Flower",
                    Description = "Flower"
                }
            );

            
        } 


    }
}
