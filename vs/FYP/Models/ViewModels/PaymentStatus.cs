using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class PaymentStatusInput
    {
        public string PaymentId { get; set; }
    }

    public class PaymentStatusOutput
    {
        public string Id { get; set; }
        public string PayerEmail { get; set; }
        public string PayerGivenName { get; set; }
        public string PayerSurname { get; set; }
        public string PayerId { get; set; }
        public string Status { get; set; }
        public bool PaidAll { get; set; }
    }
}
