using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("OrderItems")]
    public class OrderItem : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public Order Order { get; set; }
        public MenuItem MenuItem { get; set; }
        public double Quantity { get; set; }
    }
}
