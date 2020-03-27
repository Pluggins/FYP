using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class MyOrderDetailViewModel
    {
        public string OrderId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
