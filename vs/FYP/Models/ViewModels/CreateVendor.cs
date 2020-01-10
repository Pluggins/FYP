using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateVendorInput
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

    public class CreateVendorOutput
    {
        public string Result { get; set; }
    }
}
