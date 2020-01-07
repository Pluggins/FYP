using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class RetrieveMenuListOutput
    {
        public List<MenuItemOutput> MenuItems { get; set; }
        public string Status { get; set; }
    }

    public class RetrieveMenuListInput
    {
        public string VendorId { get; set; }
    }

    public class MenuItemOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
    }
}
