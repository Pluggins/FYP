using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models.ViewModels
{
    public class CreateUserInput
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

    }

    public class CreateUserOutput
    {
        public string Status { get; set; }
    }
}
