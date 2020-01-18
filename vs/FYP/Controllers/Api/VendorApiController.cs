using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public CreateVendorOutput Index([FromBody] CreateVendorInput input)
        {
            CreateVendorOutput output = new CreateVendorOutput();
            User staff = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            if (staff.Status < 2)
            {
                output.Result = "NO_PRIVILEGE";
            } else
            {
                if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Name))
                {
                    output.Result = "FIELD_INCOMPLETE";
                }
                else
                {
                    User user = _db._Users.Where(e => e.Email.ToLower().Equals(input.Email)).FirstOrDefault();
                    if (user == null)
                    {
                        output.Result = "USER_NOT_FOUND";
                    }
                    else
                    {
                        Vendor vendor = new Vendor()
                        {
                            Name = input.Name,
                            Email = input.Email,
                            Owner = user,
                            CreatedBy = staff.Id
                        };
                        _db.Vendors.Add(vendor);
                        _db.SaveChanges();
                        output.Result = "OK";
                    }
                }
            }
            return output;
        }

        [HttpPost]
        [Route("Api/Vendor/CheckUser")]
        public VendorCheckUserOutput CheckUser([FromBody] VendorCheckUserInput input)
        {
            VendorCheckUserOutput output = new VendorCheckUserOutput();
            User staff = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            if (staff.Status < 2)
            {
                output.Result = "NO_PRIVILEGE";
            } else
            {
                User user = _db._Users.Where(e => e.Email.ToLower().Equals(input.Email) && e.Deleted == false).FirstOrDefault();
                if (user == null)
                {
                    output.Result = "USER_NOT_FOUND";
                } else
                {
                    output.FirstName = user.FName;
                    output.LastName = user.LName;
                    output.UserID = user.Id;
                    output.Result = "OK";
                }
            }
            return output;
        }

        [HttpPost]
        [Route("Api/Vendor/RetrieveById")]
        public VendorInfoOutput RetrieveById([FromBody] VendorInfoInput input)
        {
            VendorInfoOutput output = new VendorInfoOutput();
            User staff = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            if (staff.Status < 2)
            {
                output.Result = "NO_PRIVILEGE";
            } else
            {
                Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.Id)).FirstOrDefault();
                if (vendor == null)
                {
                    output.Result = "NOT_FOUND";
                }
                else
                {
                    output.Vendor = vendor;
                    output.Result = "OK";
                }
            }
            return output;
        }

        [HttpPost]
        [Route("Api/Vendor/DeleteById")]
        public VendorInfoOutput DeleteById([FromBody] VendorInfoInput input)
        {
            VendorInfoOutput output = new VendorInfoOutput();
            User staff = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
            if (staff.Status < 2)
            {
                output.Result = "NO_PRIVILEGE";
            } else
            {
                Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.Id) && e.Deleted == false).FirstOrDefault();
                if (vendor == null)
                {
                    output.Result = "NOT_FOUND";
                }
                else
                {
                    vendor.Deleted = true;
                    vendor.DeletedBy = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault().Id;
                    _db.SaveChanges();
                    output.Result = "OK";
                }
            }

            return output;
        }
    }
}