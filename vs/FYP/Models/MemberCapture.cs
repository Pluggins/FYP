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
        /*
         * Type
         * 1 - Use user's session instead of temporary session before order begins
         * 2 - Update session to existing user (occurs when order is done and has not logged in)
         */
        public int Type { get; set; }
        public virtual AppLoginSession AppLoginSession { get; set; }
        public int Status { get; set; }

        public MemberCapture()
        {
            Id = Guid.NewGuid().ToString();
            string key = Guid.NewGuid().ToString();
            Code = HashingService.GenerateSHA256(Convert.FromBase64String(key.Replace("-", "")), Convert.FromBase64String(ApplicationDbContext._hashSalt)).Replace('+','x');
            Type = 0;
            Status = 1;
        }
    }
}
