using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class OrderPaymentViewModel
    {
        public Order Order { get; set; }
        public Vendor SelectedVendor { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
