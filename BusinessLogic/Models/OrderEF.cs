using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Models
{
    public class OrderEF
    {
        public OrderEF()
        {

        }

        public int OrderQuantity { get; set; }

        public string PaymentMethod { get; set; }

        public decimal TotalGrossValue { get; set; }
    }
}
