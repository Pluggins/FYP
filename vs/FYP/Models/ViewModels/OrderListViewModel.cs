using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class OrderListViewModel
    {
        public Vendor SelectedVendor { get; set; }
        public List<Order> OrderList { get; set; }
        public List<Vendor> VendorList { get; set; }
    }
}
