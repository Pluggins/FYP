using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class _CommonAttributes
    {
        public string CreatedBy { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Deleted { get; set; }

        public _CommonAttributes()
        {
            CreatedBy = "SYSTEM";
            DateCreated = DateTime.UtcNow.AddHours(8);
            Deleted = false;
        }
    }
}
