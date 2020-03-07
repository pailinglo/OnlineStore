using System;
using System.Collections.Generic;
using System.Text;
using OnlineStore.Models;

namespace OnlineStore.Services
{
    public interface ICategoryRepository
    {

        IEnumerable<Category> GetAllCategories();
        Category GetCategory(int Id);
        
    }
}
