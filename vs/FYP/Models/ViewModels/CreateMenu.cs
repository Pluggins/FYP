using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateMenuInput
    {
        public string VendorId { get; set; }
        public string MenuName { get; set; }
    }

    public class CreateMenuOutput
    {
        public string Result { get; set; }
    }
}
