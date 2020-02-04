using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    [Authorize]
    public class OrderApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrderApiController(ApplicationDbContext db)
        {
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Order/Create")]
        public CreateOrderOutput Create([FromBody] CreateOrderInput input)
        {
            CreateOrderOutput output = new CreateOrderOutput();
            Order newOrder = new Order();
            bool orderCreated = false;
            Vendor selectedVendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId)).FirstOrDefault();

            if (selectedVendor == null)
            {
                output.Status = "INCORRECT_VENDOR_ID";
            } else
            {
                output.VendorId = selectedVendor.Id;
                newOrder.Vendor = selectedVendor;

                // Advised not to be used
                if (input.Type == 1)
                {
                    output.OrderId = newOrder.Id;
                    output.Status = "OK";
                    _db.Orders.Add(newOrder);
                    _db.SaveChanges();
                    orderCreated = true;

                }
                else if (input.Type == 2)
                {
                    SessionService session = new SessionService(_db, input.SessionId, input.SessionKey);
                    if (session.IsValid)
                    {
                        User selectedUser = session.User;
                        output.OrderId = newOrder.Id;
                        output.Status = "OK";
                        output.UserId = selectedUser.Id;
                        newOrder.User = session.User;
                        _db.Orders.Add(newOrder);
                        _db.SaveChanges();
                        orderCreated = true;
                    }
                    else
                    {
                        output.Status = "SESSION_NOT_FOUND_OR_EXPIRED";
                    }
                }
            }

            if (orderCreated)
            {
                Order selectedOrder = _db.Orders.Where(e => e.Id.Equals(newOrder.Id)).FirstOrDefault();
                if (input.Items != null)
                {
                    foreach (CreateOrderItem item in input.Items)
                    {
                        MenuItem orderItem = _db.MenuItems.Where(e => e.Id.Equals(item.ItemId)).FirstOrDefault();
                        if (orderItem != null)
                        {
                            if (selectedOrder.OrderItems.Where(e => e.MenuItem == orderItem).Count() <= 0)
                            {
                                selectedOrder.OrderItems.Add(new OrderItem()
                                {
                                    Order = newOrder,
                                    MenuItem = orderItem,
                                    Quantity = item.Quantity
                                });
                            }
                            else
                            {
                                selectedOrder.OrderItems.Where(e => e.MenuItem == orderItem).FirstOrDefault().Quantity += item.Quantity;
                            }
                            _db.SaveChanges();
                        }
                        newOrder.OrderItems.Where(e => e.MenuItem.Id.Equals(orderItem.Id)).Count();
                    }
                }
            }
            return output;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Order/SelfList")]
        public List<RetrieveOrderListOutput> RetrieveOrderList_Self([FromBody] RetrieveOrderListInput input)
        {
            SessionService session = new SessionService(_db, input.SessionId, input.SessionKey);
            if (session.IsValid)
            {
                List<Order> orders = _db._Users.Where(e => e.Id.Equals(session.User.Id)).FirstOrDefault().ListOrders.Where(e => e.Deleted == false).ToList();
                List<RetrieveOrderListOutput> output = new List<RetrieveOrderListOutput>();
                foreach(Order item in orders)
                {
                    RetrieveOrderListOutput sOutput = new RetrieveOrderListOutput()
                    {
                        OrderId = item.Id,
                        Date = item.DateCreated,
                        Status = item.Status,
                        Amount = item.Amount,
                        VendorName = item.Vendor.Name
                    };
                    output.Add(sOutput);
                }

                return output;
            } else
            {
                return null;
            }
        }
    }
}