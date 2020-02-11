using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    [Authorize]
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Vendor> vendorList = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault().ListVendors.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
            if (vendorList.Count == 0)
            {
                ViewBag.Nav = 2;
                return View();
            } else
            {
                return RedirectToAction("Edit", "Menu", new { id = vendorList.First().Id });
            }
            
        }

        [Route("Menu/{id}")]
        public IActionResult Edit(string id) 
        {
            Vendor vendor = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault().ListVendors.Where(e => e.Id.Equals(id) && e.Deleted == false).FirstOrDefault();
            if (vendor == null)
            {
                return RedirectToAction("Index", "Menu");
            } else
            {
                ViewBag.Nav = 2;
                MenuListViewModel model = new MenuListViewModel();
                model.SelectedVendor = vendor;
                model.Vendors = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault().ListVendors.Where(e => e.Deleted == false).OrderBy(e => e.DateCreated).ToList();
                model.Menus =  vendor.Menus.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
                return View(model);
            }
        }
    }
}