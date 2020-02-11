using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class OrderItemListViewModel
    {
        public List<OrderItem> OrderItems { get; set; }
        public Vendor SelectedVendor { get; set; }
        public Order Order { get; set; }
    }
}
