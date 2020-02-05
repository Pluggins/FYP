using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class CaptureApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CaptureApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/Capture/Generate")]
        public GenerateCaptureOutput Index([FromBody] GenerateCaptureInput input)
        {
            GenerateCaptureOutput output = new GenerateCaptureOutput();
            AppLoginSession session = _db.AppLoginSessions.Where(e => e.Id.Equals(input.SessionId) && e.Status == 1).FirstOrDefault();

            if (session == null)
            {
                output.Result = "SESSION_NOT_FOUND";
            } else
            {
                if (!session.Key.Equals(input.SessionKey))
                {
                    output.Result = "WRONG_KEY";
                } else
                {
                    MemberCapture mc = new MemberCapture()
                    {
                        AppLoginSession = session
                    };

                    _db.MemberCaptures.Add(mc);
                    _db.SaveChanges();

                    output.CaptureId = mc.Id;
                    output.CaptureCode = mc.Code;
                    output.Result = "OK";
                }
            }

            return output;
        }
    }
}