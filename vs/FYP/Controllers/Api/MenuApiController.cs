using System;
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
            AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (input != null)
            {
                Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId) && e.Deleted == false).FirstOrDefault();
                if (vendor == null)
                {
                    output.Status = "VENDOR_NOT_FOUND";
                } else
                {
                    if (vendor.Owner == aspUser.User || aspUser.IsStaff)
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
                    } else
                    {
                        output.Status = "NO_PRIVILEGE";
                    }
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
            Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId) && e.Deleted == false).FirstOrDefault();
            AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            CreateMenuOutput output = new CreateMenuOutput();

            if (vendor == null)
            {
                output.Result = "DOES_NOT_EXIST";
            } else
            {
                if (vendor.Owner == aspUser.User || aspUser.IsStaff)
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
            Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
            AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            MenuInfoOutput output = new MenuInfoOutput();

            if (menu == null)
            {
                output.Result = "DOES_NOT_EXIST";
            }
            else
            {
                if (menu.Vendor.Owner == aspUser.User || aspUser.IsStaff)
                {
                    output.MenuName = menu.Name;
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
            Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
            AspUserService aspUser = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            MenuInfoOutput output = new MenuInfoOutput();

            if (menu == null)
            {
                output.Result = "DOES_NOT_EXIST";
            } else
            {
                if (menu.Vendor.Owner == aspUser.User || aspUser.IsStaff)
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

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/Menu/RetrieveListByVendorId")]
        public MenuInfoOutput RetrieveListByVendorId([FromBody] MenuInfoInput input)
        {
            MenuInfoOutput output = new MenuInfoOutput();
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                if (string.IsNullOrEmpty(input.VendorId))
                {
                    output.Result = "INPUT_IS_NULL";
                }
                else
                {
                    Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId) && e.Deleted == false).FirstOrDefault();
                    if (vendor == null)
                    {
                        output.Result = "DOES_NOT_EXIST";
                    }
                    else
                    {
                        List<Menu> menuList = vendor.Menus.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
                        List<MenuInfo> newMenuList = new List<MenuInfo>();

                        foreach (Menu item in menuList)
                        {
                            MenuInfo menu = new MenuInfo()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                ShortDesc = item.ShortDesc
                            };

                            newMenuList.Add(menu);
                        }

                        output.MenuList = newMenuList;
                        output.Result = "OK";
                    }
                }
            }
            return output;
        }
    }
}