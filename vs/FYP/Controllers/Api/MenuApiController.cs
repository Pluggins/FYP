using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class MenuApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/Menu/RetrieveMenuList")]
        public RetrieveMenuListOutput RetrieveMenuList([FromBody] RetrieveMenuListInput input)
        {
            RetrieveMenuListOutput output = new RetrieveMenuListOutput();
            List<MenuItemOutput> items = new List<MenuItemOutput>();
            if (input != null)
            {
                Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId) && e.Deleted == false).FirstOrDefault();
                if (vendor == null)
                {
                    output.Status = "VENDOR_NOT_FOUND";
                } else
                {
                    List<Menu> menuList = vendor.Menus.Where(e => e.Deleted == false).ToList();
                    foreach (Menu item in menuList)
                    {
                        MenuItemOutput sOutput = new MenuItemOutput()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            ShortDesc = item.ShortDesc
                        };
                        items.Add(sOutput);
                    }
                    output.MenuItems = items;
                    output.Status = "OK";
                }
            } else
            {
                output.Status = "NO_INPUT";
            }
            return output;
        }

        [HttpPost]
        [Route("Api/Menu/Create")]
        public CreateMenuOutput CreateMenu([FromBody] CreateMenuInput input)
        {
            User user = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)) && e.Deleted == false && e.Status > 0).FirstOrDefault();
            Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId) && e.Deleted == false).FirstOrDefault();
            CreateMenuOutput output = new CreateMenuOutput();
            if (user == null || vendor == null)
            {
                output.Result = "DOES_NOT_EXIST";
            } else
            {
                if (vendor.Owner == user || user.Status > 1)
                {
                    Menu newMenu = new Menu()
                    {
                        Name = input.MenuName
                    };

                    vendor.Menus.Add(newMenu);
                    _db.SaveChanges();
                    output.Result = "OK";
                }
                else
                {
                    output.Result = "NO_PRIVILEGE";
                }
            }

            return output;
        }

        [HttpPost]
        [Route("Api/Menu/RetrieveById")]
        public MenuInfoOutput RetrieveById([FromBody] MenuInfoInput input)
        {
            User user = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)) && e.Deleted == false).FirstOrDefault();
            Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
            MenuInfoOutput output = new MenuInfoOutput();

            if (user == null || menu == null)
            {
                output.Result = "DOES_NOT_EXIST";
            }
            else
            {
                if (menu.Vendor.Owner == user || user.Status > 1)
                {
                    output.Menu = menu;
                    output.Result = "OK";
                }
                else
                {
                    output.Result = "NO_PRIVILEGE";
                }
            }

            return output;
        }

        [HttpPost]
        [Route("Api/Menu/Delete")]
        public MenuInfoOutput DeleteMenu([FromBody] MenuInfoInput input)
        {
            User user = _db._Users.Where(e => e.AspNetUser.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)) && e.Deleted == false).FirstOrDefault();
            Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
            MenuInfoOutput output = new MenuInfoOutput();

            if (user == null || menu == null)
            {
                output.Result = "DOES_NOT_EXIST";
            } else
            {
                if (menu.Vendor.Owner == user || user.Status > 1)
                {
                    menu.Deleted = true;
                    _db.SaveChanges();
                    output.Result = "OK";
                }
                else
                {
                    output.Result = "NO_PRIVILEGE";
                }
            }

            return output;
        }
    }
}