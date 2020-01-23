using FYP.Data;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Services
{
    public class AspUserService
    {
        private readonly ApplicationDbContext _db;
        public bool IsValid { get; set; }
        public bool IsStaff { get; set; }
        public User User { get; set; }

        public AspUserService(ApplicationDbContext db, string aspUserId)
        {
            _db = db;
            User = _db._Users.Where(e => e.AspNetUser.Id.Equals(aspUserId) && e.Deleted == false).FirstOrDefault();
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
                    IsStaff = true;
                }
            }
        }
    }
}
