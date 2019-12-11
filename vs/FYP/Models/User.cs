using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Status { get; set; }
        public virtual ICollection<Vendor> ListVendors { get; set; }
    }
}
