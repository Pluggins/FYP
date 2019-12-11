using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Vendors")]
    public class Vendor : _CommonAttributes
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public User Owner { get; set; }
    }
}
