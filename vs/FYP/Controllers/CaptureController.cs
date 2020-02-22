using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Services;
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
            /*
             * ViewBag status
             *  1 - Capture detail not found
             *  2 - Capture not exist
             *  3 - Detail updated to existing session
             *  4 - New session added to client
             *  5 - Detail captured to server
             */
            if (string.IsNullOrEmpty(CaptureId) || string.IsNullOrEmpty(CaptureCode))
            {
                ViewBag.Status = 1;
            } else
            {
                MemberCapture capture = _db.MemberCaptures.Where(e => e.Id.Equals(CaptureId) && e.Code.Equals(CaptureCode) && e.Deleted == false).FirstOrDefault();
                if (capture == null)
                {
                    ViewBag.Status = 2;
                }
                else
                {
                    if (capture.Status == 1)
                    {
                        AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
                        
                        if (capture.Type == 1)
                        {
                            if (aspUser.IsValid)
                            {
                                MemberCapture newCapture = _db.MemberCaptures.Where(e => e.Id.Equals(CaptureId) && e.Code.Equals(CaptureCode) && e.Deleted == false && e.Status == 1).FirstOrDefault();

                                if (newCapture == null)
                                {
                                    ViewBag.Status = 2;
                                } else
                                {
                                    AppLoginSession newSession = new AppLoginSession(Guid.NewGuid().ToString(), Request)
                                    {
                                        User = aspUser.User,
                                        Status = 1
                                    };

                                    newCapture.AppLoginSession = newSession;
                                    newCapture.Status = 2;
                                    _db.AppLoginSessions.Add(newSession);
                                    _db.SaveChanges();

                                    ViewBag.Status = 5;
                                }
                                
                            } else
                            {
                                if (Request.Cookies["CaptureId"] != null && Request.Cookies["CaptureCode"] != null)
                                {
                                    MemberCapture existingCapture = _db.MemberCaptures.Where(e => e.Id.Equals(Request.Cookies["CaptureId"].ToString()) && e.Code.Equals(Request.Cookies["CaptureCode"].ToString()) && e.Deleted == false && e.Status == 1).FirstOrDefault();

                                    if (existingCapture == null)
                                    {
                                        ViewBag.Status = 2;
                                    }
                                    else
                                    {
                                        MemberCapture newCapture = _db.MemberCaptures.Where(e => e.Id.Equals(CaptureId) && e.Code.Equals(CaptureCode) && e.Deleted == false && e.Status == 1).FirstOrDefault();
                                        newCapture.AppLoginSession = existingCapture.AppLoginSession;
                                        newCapture.Status = 2;

                                        _db.SaveChanges();
                                        ViewBag.Status = 5;
                                    }
                                }
                                else
                                {
                                    ViewBag.Status = 1;
                                }
                            }
                        }
                        else if (capture.Type == 2)
                        {
                            if (aspUser.IsValid)
                            {
                                List<Order> orders = capture.AppLoginSession.User.ListOrders.ToList();
                                foreach (Order item in orders)
                                {
                                    item.User = aspUser.User;
                                }
                                capture.Status = 2;
                                _db.SaveChanges();

                                ViewBag.Status = 3;
                            }
                            else if (Request.Cookies["CaptureId"] != null)
                            {
                                MemberCapture existingCapture = _db.MemberCaptures.Where(e => e.Id.Equals(Request.Cookies["CaptureId"].ToString())).FirstOrDefault();
                                if (existingCapture == null)
                                {
                                    ViewBag.Status = 1;
                                }
                                else
                                {
                                    if (existingCapture.Code.Equals(Request.Cookies["CaptureCode"].ToString()) && existingCapture.Status == 2)
                                    {
                                        List<Order> orders = capture.AppLoginSession.User.ListOrders.ToList();
                                        foreach (Order item in orders)
                                        {
                                            item.User = existingCapture.AppLoginSession.User;
                                        }
                                        capture.Status = 2;

                                        Response.Cookies.Append("CaptureId", existingCapture.Id,
                                        new Microsoft.AspNetCore.Http.CookieOptions()
                                        {
                                            Expires = DateTime.UtcNow.AddYears(5),
                                            HttpOnly = true,
                                            Secure = true,
                                            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                                        }); ;
                                        Response.Cookies.Append("CaptureCode", existingCapture.Code,
                                        new Microsoft.AspNetCore.Http.CookieOptions()
                                        {
                                            Expires = DateTime.UtcNow.AddYears(5),
                                            HttpOnly = true,
                                            Secure = true,
                                            SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                                        });

                                        _db.SaveChanges();
                                        ViewBag.Status = 3;
                                    }
                                    else
                                    {
                                        ViewBag.Status = 1;
                                    }
                                }
                                ViewBag.Status = 3;
                            }
                            else
                            {
                                capture.Status = 2;
                                _db.SaveChanges();
                                Response.Cookies.Append("CaptureId", capture.Id,
                                new Microsoft.AspNetCore.Http.CookieOptions()
                                {
                                    Expires = DateTime.UtcNow.AddYears(5),
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                                }); ;
                                Response.Cookies.Append("CaptureCode", capture.Code,
                                    new Microsoft.AspNetCore.Http.CookieOptions()
                                    {
                                        Expires = DateTime.UtcNow.AddYears(5),
                                        HttpOnly = true,
                                        Secure = true,
                                        SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                                    });
                                ViewBag.Status = 4;
                            }
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