using FYP.Data;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Services
{
    public class SessionService
    {
        private readonly ApplicationDbContext _db;
        private string SessionId;
        private string SessionKey;
        public bool IsValid { get; set; }
        public User User { get; set; }
        public SessionService(ApplicationDbContext db, string sessionId, string sessionKey)
        {
            _db = db;
            SessionId = sessionId;
            SessionKey = sessionKey;
            AppLoginSession session = _db.AppLoginSessions.Where(e => e.Id.Equals(SessionId) && e.Key.Equals(SessionKey) && e.Status == 1).FirstOrDefault();
            if (session == null)
            {
                IsValid = false;
            } else
            {
                IsValid = true;
                User = _db._Users.Where(e => e.Id.Equals(session.User.Id)).FirstOrDefault();
            }
        }
    }
}
