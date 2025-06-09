using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        [Key]
        public int OrderId { get; set; }

        public int ShopId { get; set; }

        public int CustomerId { get; set; }

        public int PaymentMethodId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }   
        
        public virtual Shop Shop { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
