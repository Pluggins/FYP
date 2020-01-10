using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    [Authorize]
    public class VendorApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public VendorApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/Vendor/Create")]
        public CreateVendorOutput Index(CreateVendorInput input)
        {
            CreateVendorOutput output = new CreateVendorOutput();

            if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Name))
            {
                output.Result = "FIELD_INCOMPLETE";
            } else
            {
                User user = _db._Users.Where(e => e.Email.ToLower().Equals(input.Email)).FirstOrDefault();
                if (user == null)
                {
                    output.Result = "USER_NOT_FOUND";
                } else
                {
                    Vendor vendor = new Vendor()
                    {
                        Name = input.Name,
                        Email = input.Email
                    };
                }
            }
            
            
            return output;
        }
    }
}