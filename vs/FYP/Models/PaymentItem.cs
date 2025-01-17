﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("PaymentItems")]
    public class PaymentItem : _CommonAttributes
    {
        public string Id { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual OrderItem OrderItem { get; set; }
        public double Quantity { get; set; }

        public PaymentItem()
        {
            Id = Guid.NewGuid().ToString();
            Quantity = 0;
        }
    }
}
