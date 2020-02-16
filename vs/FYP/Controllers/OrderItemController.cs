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
    public class OrderItemController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderItemController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Route("OrderItem/{id}")]
        public IActionResult Index(string id)
        {
            ViewBag.Nav = 3;
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Order");
            } else
            {
                AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
                Order order = _db.Orders.Where(e => e.Id.Equals(id) && e.Deleted == false).FirstOrDefault();

                if (order.Vendor.Owner == aspUser.User || aspUser.IsStaff)
                {
                    OrderItemListViewModel model = new OrderItemListViewModel();

                    model.SelectedVendor = order.Vendor;
                    model.Order = order;
                    if (order.OrderItems != null)
                    {
                        model.OrderItems = order.OrderItems.OrderByDescending(e => e.DateCreated).ToList();
                    }

                    return View(model);
                }
                else
                {
                    return RedirectToAction("Index", "Order");
                }
            }
        }
    }
}