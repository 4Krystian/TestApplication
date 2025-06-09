using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class Item
    {
        public Item()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int ItemId { get; set; }

        public string EAN { get; set; }

        public decimal PriceNet { get; set; }

        public decimal PriceGross { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
