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
        /*
         * status
         * 0 - closed
         * 1 - active
         * 2 - paid
         */
        public int Status { get; set; }
        public virtual User User { get; set; }
        [Required]
        public virtual Vendor Vendor { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Amount { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            Id = Guid.NewGuid().ToString();
            Status = 0;
            Amount = 0;
        }
    }
}
