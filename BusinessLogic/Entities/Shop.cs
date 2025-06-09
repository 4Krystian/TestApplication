using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Entities
{
    public class Shop
    {
        public Shop() 
        { 
            this.Orders = new HashSet<Order>();
        }

        [Key]
        public int ShopId { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
