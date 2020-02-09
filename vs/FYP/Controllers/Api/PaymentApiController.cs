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
        [Route("Api/Payment/PayAllByOrderId")]
        public PaymentInfoOutput Index(PaymentInfoInput input)
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
                        List<OrderItem> orderItems = order.OrderItems.Where(e => e.Deleted == false).ToList();
                        
                    }
                }
            }
            return output;
        }
    }
}