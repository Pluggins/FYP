using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Menus")]
    public class Menu : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public virtual Vendor Vendor { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }

        public Menu()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
