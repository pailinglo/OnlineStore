using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public Category Category { get; set; }
    }
}
