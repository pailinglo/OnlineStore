using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineStore.Models
{
    public class InventoryRecord
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        
        [Range(0,int.MaxValue)]
        public int QuantityOnHand { get; set; }
        [Range(0, int.MaxValue)]
        public int QuantityOnOrder { get; set; }
        public DateTime UpdateTime { get; set; }
    }



    public class InventoryUpdateRecord
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int QuantityChange { get; set; }
        public DateTime UpdateTime { get; set; }
        public string UpdateBy { get; set; }
        
    }
}
