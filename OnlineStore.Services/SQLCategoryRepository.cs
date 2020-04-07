using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public class SQLCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext context;

        public SQLCategoryRepository(AppDbContext context)
        {
            this.context = context;
        }
        
        public IQueryable<Category> GetAllCategories()
        {
            return context.Categories;
        }

        public Category GetCategory(int Id)
        {
            return context.Categories.Find(Id);
        }

        
    }
}
