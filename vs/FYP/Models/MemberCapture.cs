using FYP.Data;
using FYP.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("MemberCaptures")]
    public class MemberCapture : _CommonAttributes
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public virtual AppLoginSession AppLoginSession { get; set; }
        public int Status { get; set; }

        public MemberCapture()
        {
            Id = Guid.NewGuid().ToString();
            string key = Guid.NewGuid().ToString();
            Code = HashingService.GenerateSHA256(Convert.FromBase64String(key.Replace("-", "")), Convert.FromBase64String(ApplicationDbContext._hashSalt));
            Status = 1;
        }
    }
}
