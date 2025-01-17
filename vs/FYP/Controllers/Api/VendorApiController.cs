﻿using System;
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
            AspUserService aspUser = new AspUserService(_db, this);
            if (!aspUser.IsStaff)
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
                            CreatedBy = aspUser.User.Id
                        };
                        _db.Vendors.Add(vendor);
                        user.Status = 2;
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
            AspUserService aspUser = new AspUserService(_db, this);
            if (!aspUser.IsStaff)
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
            AspUserService aspUser = new AspUserService(_db, this);
            if (!aspUser.IsStaff)
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
            AspUserService aspUser = new AspUserService(_db, this);
            if (!aspUser.IsStaff)
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Vendor/RetrieveList")]
        public VendorInfoOutput RetrieveList()
        {
            VendorInfoOutput output = new VendorInfoOutput();
            List<Vendor> vendorList = _db.Vendors.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
            List<VendorInfo> newVendorList = new List<VendorInfo>();

            foreach (Vendor item in vendorList)
            {
                VendorInfo vendor = new VendorInfo()
                {
                    Id = item.Id,
                    Name = item.Name
                };

                newVendorList.Add(vendor);
            }

            output.VendorList = newVendorList;
            output.Result = "OK";
            return output;
        }
    }
}