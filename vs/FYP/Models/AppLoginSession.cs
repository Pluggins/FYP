using FYP.Data;
using FYP.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    [Table("AppLoginSessions")]
    public class AppLoginSession
    {
        [Key]
        public string Id { get; set; }
        public string Key { get; set; }
        public virtual User User { get; set; }
        /*
         * Status
         * 1 - Active
         * 2 - Member User
         */
        public int Status { get; set; }
        public DateTime DateCreated { get; set; }
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }

        public AppLoginSession()
        {

        }

        public AppLoginSession(string key, HttpRequest request)
        {
            Id = Guid.NewGuid().ToString();
            Status = 0;
            DateCreated = DateTime.UtcNow.AddHours(8);
            Key = HashingService.GenerateSHA256(Convert.FromBase64String(key.Replace("-", "")), Convert.FromBase64String(ApplicationDbContext._hashSalt));
            IpAddress = request.Host.Value;
            UserAgent = request.Headers["User-Agent"].ToString();
        }
    }
}
