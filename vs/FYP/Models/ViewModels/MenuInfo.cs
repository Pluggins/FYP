using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
    }
    public class MenuInfoInput
    {
        public string MenuId { get; set; }
        public string VendorId { get; set; }
    }

    public class MenuInfoOutput
    {
        public List<MenuInfo> MenuList { get; set; }
        public string Result { get; set; }
        public string MenuName { get; set; }
    }
}
