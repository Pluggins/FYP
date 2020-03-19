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
            decimal sum = 0;

            if (selectedVendor == null)
            {
                output.Status = "INCORRECT_VENDOR_ID";
            } else
            {
                output.VendorId = selectedVendor.Id;
                newOrder.Status = 1;
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
                selectedOrder.OrderItems = new List<OrderItem>();
                if (input.Items != null)
                {
                    foreach (CreateOrderItem item in input.Items)
                    {
                        MenuItem orderItem = _db.MenuItems.Where(e => e.Id.Equals(item.ItemId)).FirstOrDefault();
                        if (orderItem != null)
                        {
                            if (selectedOrder.OrderItems.Where(e => e.MenuItem == orderItem).Count() <= 0)
                            {
                                sum += orderItem.Price * (decimal) item.Quantity;
                                selectedOrder.OrderItems.Add(new OrderItem()
                                {
                                    Order = newOrder,
                                    MenuItem = orderItem,
                                    UnitPrice = orderItem.Price,
                                    Quantity = item.Quantity
                                });
                            }
                            else
                            {
                                selectedOrder.OrderItems.Where(e => e.MenuItem == orderItem).FirstOrDefault().Quantity += item.Quantity;
                            }
                            _db.SaveChanges();
                        }
                    }

                    newOrder.Amount = sum;
                    _db.SaveChanges();
                }
            }
            return output;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Order/AddItemByOrderId")]
        public CreateOrderOutput AddItemByOrderId([FromBody] CreateOrderInput input)
        {
            decimal sum = 0;
            CreateOrderOutput output = new CreateOrderOutput();
            if (input == null)
            {
                output.Status = "INPUT_IS_NULL";
            } else
            {
                Order order = _db.Orders.Where(e => e.Id.Equals(input.OrderId) && e.Deleted == false && e.Status == 1).FirstOrDefault();
                if (order == null)
                {
                    output.Status = "ORDER_NOT_EXIST";
                }
                else
                {
                    if (input.Items != null)
                    {
                        foreach (CreateOrderItem item in input.Items)
                        {
                            OrderItem orderItem = order.OrderItems.Where(e => e.MenuItem.Id.Equals(item.ItemId) && e.Deleted == false).FirstOrDefault();
                            MenuItem menuItem = _db.MenuItems.Where(e => e.Id.Equals(item.ItemId) && e.Deleted == false).FirstOrDefault();
                            sum += (decimal)item.Quantity * menuItem.Price;
                            if (orderItem == null)
                            {
                                OrderItem newItem = new OrderItem()
                                {
                                    Order = order,
                                    MenuItem = menuItem,
                                    UnitPrice = menuItem.Price,
                                    Quantity = item.Quantity
                                };

                                if (order.OrderItems == null)
                                {
                                    order.OrderItems = new List<OrderItem>();
                                }

                                order.OrderItems.Add(newItem);
                            }
                            else
                            {
                                orderItem.Quantity += item.Quantity;
                            }

                            output.Status = "OK";
                            _db.SaveChanges();
                        }
                    }

                    output.Status = "OK";
                    order.Amount += sum;
                    _db.SaveChanges();
                }
            }
            
            return output;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Order/AddItemByOrderIdSeparate")]
        public CreateOrderOutput AddItemByOrderIdSeparate([FromBody] CreateOrderInput input)
        {
            decimal sum = 0;
            CreateOrderOutput output = new CreateOrderOutput();
            if (input == null)
            {
                output.Status = "INPUT_IS_NULL";
            } else
            {
                Order order = _db.Orders.Where(e => e.Id.Equals(input.OrderId) && e.Deleted == false && e.Status == 1).FirstOrDefault();
                if (order == null)
                {
                    output.Status = "ORDER_NOT_EXIST";
                }
                else
                {
                    if (input.Items != null)
                    {
                        foreach (CreateOrderItem item in input.Items)
                        {
                            MenuItem menuItem = _db.MenuItems.Where(e => e.Id.Equals(item.ItemId) && e.Deleted == false).FirstOrDefault();
                            sum += (decimal)item.Quantity * menuItem.Price;
                            OrderItem newItem = new OrderItem()
                            {
                                Order = order,
                                MenuItem = menuItem,
                                UnitPrice = menuItem.Price,
                                Quantity = item.Quantity
                            };

                            if (order.OrderItems == null)
                            {
                                order.OrderItems = new List<OrderItem>();
                            }

                            order.OrderItems.Add(newItem);

                            output.Status = "OK";
                            _db.SaveChanges();
                        }
                    }

                    output.Status = "OK";
                    order.Amount += sum;
                    _db.SaveChanges();
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Order/GetActiveOrderById")]
        public OrderInfoOutput GetActiveOrderById([FromBody] OrderInfoInput input)
        {
            OrderInfoOutput output = new OrderInfoOutput();
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                Order order = _db.Orders.Where(e => e.Id.Equals(input.OrderId) && e.Deleted == false && e.Status == 1).FirstOrDefault();
                if (order == null)
                {
                    output.Result = "ORDER_NOT_EXIST";
                } else
                {
                    List<OrderInfoItem> orderInfoItems = new List<OrderInfoItem>();
                    foreach (OrderItem item in order.OrderItems.ToList())
                    {
                        if (item.QuantityPaid > 0)
                        {
                            OrderInfoItem newItem = new OrderInfoItem()
                            {
                                Name = item.MenuItem.Name,
                                OrderItemId = item.Id,
                                OrderItemUnitPrice = item.MenuItem.Price,
                                Quantity = item.QuantityPaid,
                                Status = 2
                            };

                            orderInfoItems.Add(newItem);

                            if (item.Quantity - item.QuantityPaid > 0)
                            {
                                OrderInfoItem newItem2 = new OrderInfoItem()
                                {
                                    Name = item.MenuItem.Name,
                                    OrderItemId = item.Id,
                                    OrderItemUnitPrice = item.MenuItem.Price,
                                    Quantity = item.Quantity - item.QuantityPaid,
                                    Status = 1
                                };

                                orderInfoItems.Add(newItem2);
                            }
                        } else
                        {
                            OrderInfoItem newItem = new OrderInfoItem()
                            {
                                Name = item.MenuItem.Name,
                                OrderItemId = item.Id,
                                OrderItemUnitPrice = item.MenuItem.Price,
                                Quantity = item.Quantity,
                                Status = item.Status
                            };

                            orderInfoItems.Add(newItem);
                        }
                    }

                    output.Result = "OK";
                    output.Items = orderInfoItems.OrderBy(e => e.Status).ToList();
                }
            }
            return output;
        }
    }
}