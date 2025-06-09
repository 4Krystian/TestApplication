using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public virtual Item Item { get; set; }

        public virtual Order Order { get; set; }    
    }
}
