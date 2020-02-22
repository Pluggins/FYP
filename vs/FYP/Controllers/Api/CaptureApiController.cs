using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class CaptureApiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CaptureApiController(
            ApplicationDbContext db,
            IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("Api/Capture/Generate")]
        public GenerateCaptureOutput Index([FromBody] GenerateCaptureInput input)
        {
            GenerateCaptureOutput output = new GenerateCaptureOutput();
            
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                if (input.Type == 1)
                {
                    MemberCapture mc = new MemberCapture()
                    {
                        Type = 1
                    };

                    _db.MemberCaptures.Add(mc);
                    _db.SaveChanges();

                    output.CaptureId = mc.Id;
                    output.CaptureCode = mc.Code;
                    string[] urlFrag = Request.GetDisplayUrl().Split('/');
                    output.CaptureUrl = urlFrag[0] + "//" + urlFrag[2] + "/capture?" + "CaptureId=" + output.CaptureId + "&CaptureCode=" + output.CaptureCode;
                    output.CaptureQR = urlFrag[0] + "//" + urlFrag[2] + "/qrcode/" + QRCodeService.GenerateQRCode(_hostingEnvironment, output.CaptureUrl);
                    output.Result = "OK";

                } else if (input.Type == 2)
                {
                    AppLoginSession session = _db.AppLoginSessions.Where(e => e.Id.Equals(input.SessionId) && e.Status == 1).FirstOrDefault();

                    if (session == null)
                    {
                        output.Result = "SESSION_NOT_FOUND";
                    }
                    else
                    {
                        if (!session.Key.Equals(input.SessionKey))
                        {
                            output.Result = "WRONG_KEY";
                        }
                        else
                        {
                            MemberCapture mc = new MemberCapture()
                            {
                                AppLoginSession = session,
                                Type = 2
                            };

                            _db.MemberCaptures.Add(mc);
                            _db.SaveChanges();

                            string[] urlFrag = Request.GetDisplayUrl().Split('/');

                            output.CaptureId = mc.Id;
                            output.CaptureCode = mc.Code;
                            output.CaptureUrl = urlFrag[0] + "//" + urlFrag[2] + "/capture?" + "CaptureId=" + output.CaptureId + "&CaptureCode=" + output.CaptureCode;
                            output.CaptureQR = urlFrag[0] + "//" + urlFrag[2] + "/qrcode/" + QRCodeService.GenerateQRCode(_hostingEnvironment, output.CaptureUrl);
                            output.Result = "OK";
                        }
                    }
                } else
                {
                    output.Result = "TYPE_NOT_EXIST";
                }
            }
            
            

            return output;
        }

        [HttpPost]
        [Route("Api/Capture/GetCaptureStatus")]
        public CaptureInfoOutput GetCaptureStatus([FromBody] CaptureInfoInput input)
        {
            CaptureInfoOutput output = new CaptureInfoOutput();
            MemberCapture capture = _db.MemberCaptures.Where(e => e.Id.Equals(input.CaptureId)).FirstOrDefault();

            if (capture == null)
            {
                output.Status = "CAPTURE_NOT_EXIST";
            } else
            {
                if (capture.Status == 1)
                {
                    output.Status = "ACTIVE";
                } else if (capture.Status == 2)
                {
                    if (capture.AppLoginSession != null && capture.Type == 1)
                    {
                        output.SessionId = capture.AppLoginSession.Id;
                        output.SessionKey = capture.AppLoginSession.Key;
                    }
                    output.Status = "SCANNED";
                } else
                {
                    output.Status = "EXPIRED";
                }
            }

            return output;
        }
    }
}