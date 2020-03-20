using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            AspUserService aspUser = new AspUserService(_db, this);
            List<Vendor> vendorList = aspUser.User.ListVendors.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
            if (vendorList.Count == 0)
            {
                ViewBag.Nav = 3;
                return View();
            } else
            {
                return RedirectToAction("Edit", "Order", new { id = vendorList.First().Id });
            }
        }

        [Route("/Order/{id}")]
        public IActionResult Edit(string id)
        {
            ViewBag.Nav = 3;
            OrderListViewModel model = new OrderListViewModel();
            AspUserService aspUser = new AspUserService(_db, this);
            List<Vendor> vendorList = aspUser.User.ListVendors.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();

            model.VendorList = vendorList;
            model.SelectedVendor = vendorList.Where(e => e.Id.Equals(id)).FirstOrDefault();
            model.OrderList = model.SelectedVendor.Orders.Where(e => e.Deleted == false).OrderByDescending(e => e.DateCreated).ToList();
            
            return View(model);
        }
    }
}