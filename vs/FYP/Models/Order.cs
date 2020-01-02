using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Orders")]
    public class Order : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public int Status { get; set; }
        public User User { get; set; }
        public decimal Amount { get; set; }
    }
}
