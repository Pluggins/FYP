using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class PaymentInfoInput
    {
        public string OrderId { get; set; }
    }

    public class PaymentInfoOutput
    {
        public string PaymentId { get; set; }
        public string PaymentLink { get; set; }
        public string PaymentLinkQR { get; set; }
        public string Result { get; set; }
        public decimal Amount { get; set; }
    }
}
