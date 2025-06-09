using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class Customer
    {
        public Customer() 
        {
            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int CustomerId { get; set; }

        public string Adress { get; set; }
        public string PostalCode { get; set; }

        public string Town { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
