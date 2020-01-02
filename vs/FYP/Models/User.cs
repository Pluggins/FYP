﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Users")]
    public class User : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public int Status { get; set; }
        [Required]
        public IdentityUser AspNetUser { get; set; }
        public virtual ICollection<Vendor> ListVendors { get; set; }

        public User()
        {
            Id = Guid.NewGuid().ToString();
            Status = 0;
        }
    }
}
