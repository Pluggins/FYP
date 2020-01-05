using System;
using System.Collections.Generic;
using System.Text;
using FYP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FYP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public static byte[] _hashSalt = Convert.FromBase64String("6cm+(w&SK[BQ[~]Bvauyt~+)tSTZV5u<u('&{!]dz8v'-LTNPt?Bx2p)y@:/$u'R");
        public DbSet<User> _Users { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AppLoginSession> AppLoginSessions { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
             
        }
    }
}
