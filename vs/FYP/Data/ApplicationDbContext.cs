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
        public DbSet<User> ApplicationUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
             
        }
    }
}
