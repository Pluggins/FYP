using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateMenuItemInput
    {
        public string MenuId { get; set; }
        public string ItemName { get; set; }
        public string Price { get; set; }
        public string Desc { get; set; }
        public double WaitingTime { get; set; }
        public string ImgUrl { get; set; }

        public CreateMenuItemInput()
        {
            WaitingTime = 0;
        }
    }

    public class CreateMenuItemOutput
    {
        public string Result { get; set; }
    }
}
