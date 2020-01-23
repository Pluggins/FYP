using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuItemListViewModel
    {
        public List<MenuItem> MenuItems { get; set; }
        public Vendor Vendor { get; set; }
        public Menu Menu { get; set; }
    }
}
