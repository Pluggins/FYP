using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("MenuItems")]
    public class MenuItem : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public virtual Menu Menu { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }

        public MenuItem()
        {
            Id = Guid.NewGuid().ToString();
            Price = 0;
        }
    }
}
