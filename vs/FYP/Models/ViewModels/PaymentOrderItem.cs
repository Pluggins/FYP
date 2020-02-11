using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class PaymentOrderItem
    {
        public string Id { get; set; }
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
