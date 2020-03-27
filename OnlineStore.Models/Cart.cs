using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineStore.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        //virtual: lazy loading in Entity Framework =>EF loads this property from database when it is accessed the first time.
        public virtual Product Product { get; set; }
    }
}
