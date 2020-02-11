using System;
using System.Collections.Generic;
using System.Text;
using FYP.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FYP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public static string _hashSalt = "XE6uulOqnjQyAwr6YQykF4AMvcQSBOWnjSHB4kQftwCRQTLCzaM22bFndj5DWPO3";
        public DbSet<User> _Users { get; set; }
        public DbSet<IdentityUser> _AspNetUsers { get; set; }
        public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentItem> PaymentItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<AppLoginSession> AppLoginSessions { get; set; }
        public DbSet<MemberCapture> MemberCaptures { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique(true);
        }
    }
}
