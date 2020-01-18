using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class VendorInfoOutput
    {
        public Vendor Vendor { get; set; }
        public string Result { get; set; }
    }

    public class VendorInfoInput
    {
        public string Id { get; set; }
    }
}
