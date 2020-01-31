using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuItemInfoInput
    {
        public string MenuItemId { get; set; }
    }

    public class MenuItemInfoOutput
    {
        public string Result { get; set; }
        public string MenuName { get; set; }
    }
}
