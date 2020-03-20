using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class OrderPreviousItem
    {
        public string OrderId { get; set; }
        public string OrderDate { get; set; }
        public decimal Price { get; set; }
    }
}
