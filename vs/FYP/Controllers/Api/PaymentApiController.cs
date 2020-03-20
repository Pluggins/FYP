using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FYP.Controllers.Api
{
    public class PaymentApiController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PaymentApiController(
            ApplicationDbContext db,
            IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("Api/Payment/PayRestByOrderId")]
        public async Task<PaymentInfoOutput> PayRestByOrderId([FromBody] PaymentInfoInput input)
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
                            List<PaymentItem> paymentItems = new List<PaymentItem>();
                            decimal sum = 0;

                            foreach (OrderItem item in orderItems.Where(e => e.Status == 1))
                            {
                                PaymentItem pItem = new PaymentItem()
                                {
                                    OrderItem = orderItems.Where(e => e.Id.Equals(item.Id)).FirstOrDefault(),
                                    Quantity = item.Quantity - item.QuantityPaid
                                };

                                sum += item.UnitPrice * (decimal) (item.Quantity - item.QuantityPaid);
                                paymentItems.Add(pItem);
                                _db.PaymentItems.Add(pItem);
                            }

                            if (sum > 0)
                            {
                                PaymentServiceOutput paymentService = await PaymentService.InitPaypalAsync(sum);
                                Payment newPayment = new Payment()
                                {
                                    Amount = sum,
                                    Status = 1,
                                    Order = order,
                                    Method = "PAYPAL",
                                    MethodId = paymentService.PaymentId
                                };
                                newPayment.PaymentItems = paymentItems;

                                _db.Payments.Add(newPayment);
                                _db.SaveChanges();
                                output.Amount = sum;
                                output.Result = "OK";
                                output.PaymentId = newPayment.Id;
                                output.PaymentLink = paymentService.PaymentLink;
                                string[] urlFrag = Request.GetDisplayUrl().Split('/');
                                output.PaymentLinkQR = urlFrag[0]+"//"+urlFrag[2] + "/qrcode/" + QRCodeService.GenerateQRCode(_hostingEnvironment, output.PaymentLink);
                            }
                            else
                            {
                                order.Status = 2;
                                _db.SaveChanges();
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

        [HttpPost]
        [Route("Api/Payment/PaySelectiveByOrderItemId")]
        public async Task<PaymentInfoOutput> PaySelectiveByOrderItemId([FromBody] PaymentInfoInput input)
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
                            List<PaymentItem> paymentItems = new List<PaymentItem>();
                            decimal sum = 0;

                            foreach (PaymentInfoItem item in input.OrderItems)
                            {
                                OrderItem existingOrderItem = order.OrderItems.Where(e => e.Id.Equals(item.OrderItemId) && e.Status == 1 && e.Deleted == false).FirstOrDefault();
                                if (existingOrderItem != null)
                                {
                                    sum += existingOrderItem.UnitPrice * item.Quantity;
                                    PaymentItem pItem = new PaymentItem()
                                    {
                                        OrderItem = existingOrderItem,
                                        Quantity = item.Quantity
                                    };
                                    paymentItems.Add(pItem);
                                }
                            }

                            if (sum > 0)
                            {
                                PaymentServiceOutput paymentService = await PaymentService.InitPaypalAsync(sum);
                                Payment newPayment = new Payment()
                                {
                                    Amount = sum,
                                    Status = 1,
                                    Order = order,
                                    Method = "PAYPAL",
                                    MethodId = paymentService.PaymentId
                                };
                                _db.Payments.Add(newPayment);

                                newPayment.PaymentItems = paymentItems;

                                _db.SaveChanges();
                                output.Amount = sum;
                                output.Result = "OK";
                                output.PaymentId = newPayment.Id;
                                output.PaymentLink = paymentService.PaymentLink;
                                string[] urlFrag = Request.GetDisplayUrl().Split('/');
                                output.PaymentLinkQR = urlFrag[0]+"//"+urlFrag[2] + "/qrcode/" + QRCodeService.GenerateQRCode(_hostingEnvironment, output.PaymentLink);
                            }
                            else
                            {
                                order.Status = 2;
                                _db.SaveChanges();
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
        
        [HttpPost]
        [Route("Api/Payment/CheckPaypalOrder")]
        public async Task<PaymentStatusOutput> CheckPaypalOrder([FromBody] PaymentStatusInput input)
        {
            PaymentStatusOutput output = new PaymentStatusOutput();
            if (input == null)
            {
                output.Status = "INPUT_IS_NULL";
            } else
            {
                Payment payment = _db.Payments.Where(e => e.Deleted == false && e.Id.Equals(input.PaymentId)).FirstOrDefault();
                
                if (payment == null)
                {
                    output.Status = "PAYMENT_DONE_OR_EXPIRED";
                }
                else
                {

                    output = await PaymentService.CheckPaypal(payment.MethodId);
                    if (output.Status == "APPROVED")
                    {
                        List<PaymentItem> paymentItems = payment.PaymentItems.Where(e => e.Deleted == false).ToList();
                        List<OrderItem> orderItems = _db.Orders.Where(e => e.Id.Equals(payment.Order.Id) && e.Deleted == false).FirstOrDefault().OrderItems.Where(e => e.Deleted == false).ToList();
                        bool paidAll = true;
                        if (payment.Status != 2)
                        {
                            foreach (PaymentItem item in paymentItems)
                            {
                                OrderItem tmpItem = orderItems.Where(e => e.Id.Equals(item.OrderItem.Id) && e.Deleted == false).FirstOrDefault();
                                double unpaidQuantity = tmpItem.Quantity - tmpItem.QuantityPaid;
                                if (item.Quantity >= unpaidQuantity)
                                {
                                    item.OrderItem.Status = 2;
                                    item.OrderItem.QuantityPaid += item.Quantity;
                                }
                                else
                                {
                                    item.OrderItem.QuantityPaid += item.Quantity;
                                }

                            }
                            payment.Status = 2;
                            _db.SaveChanges();
                        }

                        foreach (OrderItem item in payment.Order.OrderItems.Where(e => e.Deleted == false))
                        {
                            if (item.Status == 1)
                            {
                                paidAll = false;
                            }
                        }
                        
                        if (paidAll)
                        {
                            payment.Order.Status = 2;
                            _db.SaveChanges();
                            output.PaidAll = true;
                        } else
                        {
                            output.PaidAll = false;
                        }
                    }
                }
            }
            
            return output;
        }
    }
}