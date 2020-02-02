using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class MenuItemInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public string Price { get; set; }
    }

    public class MenuItemInfoInput
    {
        public string MenuId { get; set; }
        public string MenuItemId { get; set; }
    }

    public class MenuItemInfoOutput
    {
        public string Result { get; set; }
        public string MenuName { get; set; }
        public List<MenuItemInfo> MenuItemList { get; set; }
    }
}
