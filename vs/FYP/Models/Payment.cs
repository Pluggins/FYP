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
        public virtual Order Order { get; set; }
        public string NickName { get; set; }
        public decimal Amount { get; set; }
        /*
         * status
         * 0 - closed
         * 1 - active
         * 2 - done
         */
        public int Status { get; set; }
        public virtual ICollection<PaymentItem> PaymentItems { get; set; }

        public Payment()
        {
            Id = Guid.NewGuid().ToString();
            Amount = 0;
            Status = 0;
        }
    }
}
