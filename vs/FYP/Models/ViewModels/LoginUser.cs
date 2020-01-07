using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class LoginUserInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginUserOutput
    {
        public string SessionId { get; set; }
        public string Key { get; set; }
        public string Result { get; set; }
    }
}
