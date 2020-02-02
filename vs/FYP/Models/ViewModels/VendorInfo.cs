using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class VendorInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class VendorInfoOutput
    {
        public List<VendorInfo> VendorList { get; set; }
        public Vendor Vendor { get; set; }
        public string Result { get; set; }
    }

    public class VendorInfoInput
    {
        public string Id { get; set; }
    }
}
