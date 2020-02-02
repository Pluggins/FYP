using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    [Authorize]
    public class VendorController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VendorController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Vendor> vendorList = new List<Vendor>();
            vendorList = _db.Vendors.Where(e => e.Deleted == false).OrderByDescending(e => e.DateCreated).ToList();
            ViewBag.Nav = 1;
            return View(vendorList);
        }
    }
}