using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("Payments")]
    public class Payment : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public Order Order { get; set; }
        public string NickName { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
    }
}
