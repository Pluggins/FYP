using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public partial class ApplicationUser : IdentityUser
    {
        public User User { get; set; }
    }
}
