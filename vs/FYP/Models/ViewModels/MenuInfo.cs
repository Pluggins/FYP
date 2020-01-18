using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuInfoInput
    {
        public string MenuId { get; set; }
    }

    public class MenuInfoOutput
    {
        public string Result { get; set; }
        public Menu Menu { get; set; }
    }
}
