using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class VendorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            } else
            {
                ViewBag.Nav = 1;
            }
            return View();
        }
    }
}