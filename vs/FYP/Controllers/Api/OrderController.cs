using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/Order/Create")]
        public CreateOrderOutput CreateOrder(CreateOrderInput input)
        {
            CreateOrderOutput output = new CreateOrderOutput();
            Order newOrder = new Order();
            if (input.Type == 1)
            {
                output.OrderId = newOrder.Id;
                output.Status = "OK";
                _db.Orders.Add(newOrder);
                _db.SaveChanges();

            } else if (input.Type == 2)
            {
                User selectedUser = _db.ApplicationUsers.Where(e => e.Id == input.MemberId).FirstOrDefault();
                if (selectedUser == null)
                {
                    output.Status = "USER_NOT_FOUND";
                } else
                {
                    output.OrderId = newOrder.Id;
                    output.Status = "OK";
                    output.User = selectedUser;
                    _db.Orders.Add(newOrder);
                    _db.SaveChanges();
                }
            }
            return output;
        }
    }
}