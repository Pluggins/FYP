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
    public class MenuItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuItemController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("MenuItem/{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.Nav = 2;
            MenuItemListViewModel model = new MenuItemListViewModel();
            model.Menu = _db.Menus.Where(e => e.Id.Equals(id) && e.Deleted == false).FirstOrDefault();
            AspUserService userService = new AspUserService(_db, this);
            if (model.Menu == null)
            {
                return RedirectToAction("Index", "Menu");
            } else
            {
                if (userService.User == model.Menu.Vendor.Owner || userService.IsStaff)
                {
                    model.MenuItems = model.Menu.MenuItems.Where(e => e.Deleted == false).OrderByDescending(e => e.Name).ToList();
                    model.Vendor = model.Menu.Vendor;
                    return View(model);
                } else
                {
                    return RedirectToAction("Index", "Menu");
                }
            }
            
        }
    }
}