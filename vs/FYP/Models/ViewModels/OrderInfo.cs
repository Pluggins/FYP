using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class OrderInfoInput
    {
        public string OrderId { get; set; }
    }
    public class OrderInfoOutput
    {
        public List<OrderInfoItem> Items { get; set; }
        public string Result { get; set; }
    }

    public class OrderInfoItem
    {
        public string OrderItemId { get; set; }
        public string MenuItemId { get; set; }
        public decimal OrderItemUnitPrice { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public int Status { get; set; }
    }
}
