using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateOrderOutput
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public User User { get; set; }
    }
}
