using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class RetrieveOrderListInput
    {
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
    }

    public class RetrieveOrderListOutput
    {
        public string OrderId { get; set; }
        public DateTime Date { get; set; }
        public int Status { get; set; }
        public decimal Amount { get; set; }
    }
}
