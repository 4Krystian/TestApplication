using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class OrderWebAPI
    {
        public OrderWebAPI()
        {

        }

        public int OrderId { get; set; }

        public int ShopId { get; set; }

        public string Shop { get; set; }

        public string Town { get; set; }

        public decimal TotalNetValue { get; set; }
    }
}
