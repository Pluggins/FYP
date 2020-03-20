using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FYP.Services
{
    public class AspUserService
    {
        private readonly ApplicationDbContext _db;
        public bool IsValid { get; set; }
        public bool IsStaff { get; set; }
        public bool IsVendor { get; set; }
        public User User { get; set; }

        public AspUserService(ApplicationDbContext db, Controller controller)
        {
            _db = db;
            if (controller.User.Identity.IsAuthenticated)
            {
                User = _db._Users.Where(e => e.AspNetUser.Id.Equals(controller.User.FindFirstValue(ClaimTypes.NameIdentifier)) && e.Deleted == false).FirstOrDefault();
            } else
            {
                User = null;
            }
            
            IsValid = false;
            IsStaff = false;

            if (User != null)
            {
                if (User.Status > 0)
                {
                    IsValid = true;
                }

                if (User.Status > 1)
                {
                    IsVendor = true;
                }

                if (User.Status > 2)
                {
                    IsStaff = true;
                }
            }
        }
    }
}
