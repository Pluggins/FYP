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
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual ICollection<PaymentItem> PaymentItems { get; set; }
        public double Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public double Remark { get; set; }

        public OrderItem()
        {
            Id = Guid.NewGuid().ToString();
            Quantity = 0;
        }
    }
}
