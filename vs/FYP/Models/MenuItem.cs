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
        public Menu Menu { get; set; }
        public decimal Price { get; set; }
        ICollection<PaymentItem> PaymentItems { get; set; }

        public MenuItem()
        {
            Id = Guid.NewGuid().ToString();
            Price = 0;
        }
    }
}
