using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICategoryRepository
    {

        IQueryable<Category> GetAllCategories();
        Category GetCategory(int Id);
        
    }
}
