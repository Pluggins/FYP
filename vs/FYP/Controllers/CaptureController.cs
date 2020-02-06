using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class CaptureController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CaptureController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string CaptureId, string CaptureCode)
        {
            if (string.IsNullOrEmpty(CaptureId) || string.IsNullOrEmpty(CaptureCode))
            {
                ViewBag.Status = 1;
            } else
            {
                MemberCapture capture = _db.MemberCaptures.Where(e => e.Id.Equals(CaptureId) && e.Deleted == false).FirstOrDefault();
                if (capture == null)
                {
                    ViewBag.Status = 2;
                }
                else
                {
                    if (capture.Status == 1)
                    {
                        if (Request.Cookies["CaptureId"] != null)
                        {
                            ViewBag.Status = 3;
                        }
                        else
                        {
                            Response.Cookies.Append("CaptureId", capture.Id,
                            new Microsoft.AspNetCore.Http.CookieOptions()
                            {
                                Expires = DateTime.UtcNow.AddHours(8).AddYears(2),
                                HttpOnly = true,
                                Secure = true,
                                SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                            }); ;
                            Response.Cookies.Append("CaptureCode", capture.Code,
                                new Microsoft.AspNetCore.Http.CookieOptions()
                                {
                                    Expires = DateTime.UtcNow.AddHours(8).AddYears(2),
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                                });
                            ViewBag.Status = 4;
                        }
                    }
                    else
                    {
                        ViewBag.Status = 2;
                    }
                }
            }
            return View();
        }
    }
}