using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Controllers
{
    [Authorize]
    public class MyOrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MyOrderController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.Nav = 4;
            AspUserService aspUser = new AspUserService(_db, this);

            if (aspUser.IsValid)
            {
                List<Order> orders = aspUser.User.ListOrders.Where(e => e.Deleted == false).ToList();
                MyOrderViewModel model = new MyOrderViewModel()
                {
                    Orders = orders
                };

                return View(model);
            } else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("MyOrder/{id}")]
        public IActionResult Detail(string id)
        {
            ViewBag.Nav = 4;
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Home");
            } else
            {
                AspUserService aspUser = new AspUserService(_db, this);

                if (aspUser.IsValid)
                {
                    MyOrderDetailViewModel model = new MyOrderDetailViewModel();
                    List<OrderItem> orderItems = new List<OrderItem>();

                    Order order = aspUser.User.ListOrders.Where(e => e.Deleted == false && e.Id.Equals(id)).FirstOrDefault();
                    foreach (OrderItem item in order.OrderItems.Where(e => e.Deleted == false))
                    {
                        orderItems.Add(item);
                    }

                    model.OrderId = order.Id;
                    model.OrderItems = orderItems;
                    return View(model);
                } else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }
    }
}
