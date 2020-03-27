using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineStore.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }

        [Required]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
    }
}
