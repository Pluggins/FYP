using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuListViewModel
    {
        public List<Menu> Menus { get; set; }
        public List<Vendor> Vendors { get; set; }
        public Vendor SelectedVendor { get; set; }
    }
}
