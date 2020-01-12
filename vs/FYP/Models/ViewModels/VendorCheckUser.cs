using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class VendorCheckUserInput
    {
        public string Email { get; set; }
    }

    public class VendorCheckUserOutput
    {
        public string Result { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserID { get; set; }
    }
}
