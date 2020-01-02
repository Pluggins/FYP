using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Vendors")]
    public class Vendor : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public User Owner { get; set; }
        public ICollection<Menu> List_Menu { get; set; }

        public Vendor()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
