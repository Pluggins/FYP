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
                        } else if (Request.Cookies["CaptureId"] != null)
                        {
                            MemberCapture existingCapture = _db.MemberCaptures.Where(e => e.Id.Equals(Request.Cookies["CaptureId"].ToString())).FirstOrDefault();
                            if (existingCapture == null)
                            {
                                ViewBag.Status = 1;
                            } else
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
                                } else
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