using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Services;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.IsMobile = MobileDetectService.IsMobile(this);
            return View();
        }
    }
}