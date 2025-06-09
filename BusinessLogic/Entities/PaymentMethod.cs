using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int PaymentMethodId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
