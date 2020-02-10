using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class PaymentApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaymentApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/Payment/PayRestByOrderId")]
        public PaymentInfoOutput PayRestByOrderId(PaymentInfoInput input)
        {
            PaymentInfoOutput output = new PaymentInfoOutput();
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                if (string.IsNullOrEmpty(input.OrderId))
                {
                    output.Result = "INPUT_IS_NULL";
                } else
                {
                    Order order = _db.Orders.Where(e => e.Id.Equals(input.OrderId) && e.Deleted == false).FirstOrDefault();
                    if (order == null)
                    {
                        output.Result = "ORDER_DOES_NOT_EXIST";
                    } else
                    {
                        if (order.Status == 1)
                        {
                            List<OrderItem> orderItems = order.OrderItems.Where(e => e.Deleted == false).ToList();
                            List<OrderItem> currentOrderItems = orderItems;
                            List<Payment> payments = order.Payments.Where(e => e.Status == 2).ToList();
                            List<PaymentItem> paymentItems = new List<PaymentItem>();
                            decimal sum = 0;

                            foreach (Payment item in payments)
                            {
                                List<PaymentItem> tmpPaymentItem = item.PaymentItems.ToList();
                                foreach (PaymentItem subItem in tmpPaymentItem)
                                {
                                    paymentItems.Add(subItem);
                                }
                            }

                            foreach (PaymentItem item in paymentItems)
                            {
                                OrderItem orderItem = currentOrderItems.Where(e => e.Id.Equals(item.OrderItem.Id)).FirstOrDefault();
                                if (orderItem != null)
                                {
                                    orderItem.Quantity -= item.Quantity;
                                }
                            }

                            foreach (OrderItem item in currentOrderItems)
                            {
                                sum += item.UnitPrice * (decimal)item.Quantity;
                            }

                            if (sum > 0)
                            {
                                Payment newPayment = new Payment()
                                {
                                    Amount = sum,
                                    Status = 1,
                                    Order = order
                                };

                                foreach (OrderItem item in currentOrderItems)
                                {
                                    if (item.Quantity > 0)
                                    {
                                        PaymentItem pItem = new PaymentItem()
                                        {
                                            OrderItem = item,
                                            Quantity = item.Quantity
                                        };

                                        newPayment.PaymentItems.Add(pItem);
                                    }
                                }

                                _db.SaveChanges();
                                output.Amount = sum;
                                output.Result = "OK";
                                output.PaymentId = newPayment.Id;
                            }
                            else
                            {
                                output.Amount = sum;
                                output.Result = "PAID";
                            }
                        } else
                        {
                            output.Result = "PAID_OR_EXPIRED";
                        }
                    }
                }
            }
            return output;
        }
    }
}