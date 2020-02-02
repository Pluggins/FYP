using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateOrderInput
    {
        public int Type { get; set; }
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
        public List<CreateOrderItem> Items { get; set; }

        public CreateOrderInput()
        {
            Type = 0;
        }
    }

    public class CreateOrderOutput
    {
        public string OrderId { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
    }

    public class CreateOrderItem
    {
        public string ItemId { get; set; }
        public double Quantity { get; set; }
        public CreateOrderItem()
        {
            Quantity = 0;
        }
    }
}
