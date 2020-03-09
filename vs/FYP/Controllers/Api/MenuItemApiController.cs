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
    public class MenuItemApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public MenuItemApiController(
            ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("Api/MenuItem/Create")]
        public CreateMenuItemOutput CreateMenuItem([FromBody] CreateMenuItemInput input)
        {
            CreateMenuItemOutput output = new CreateMenuItemOutput();
            AspUserService userService = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
            
            if (menu == null)
            {
                output.Result = "DOES_NOT_EXIST";
            } else
            {
                if (menu.Vendor.Owner == userService.User || userService.IsStaff)
                {
                    decimal inputPrice;
                    if (decimal.TryParse(input.Price, out inputPrice))
                    {
                        MenuItem newItem = new MenuItem()
                        {
                            Name = input.ItemName,
                            ShortDesc = input.Desc,
                            Menu = menu,
                            Price = inputPrice
                        };

                        _db.MenuItems.Add(newItem);
                        _db.SaveChanges();

                        output.Result = "OK";
                    } else
                    {
                        output.Result = "PARSING_ERROR";
                    }
                } else
                {
                    output.Result = "NO_PRIVILEGE";
                }
            }

            return output;
        }

        [HttpPost]
        [Route("Api/MenuItem/Remove")]
        public MenuItemInfoOutput RemoveMenuItem([FromBody] MenuItemInfoInput input)
        {
            MenuItemInfoOutput output = new MenuItemInfoOutput();
            if (!string.IsNullOrEmpty(input.MenuItemId))
            {
                MenuItem menuItem = _db.MenuItems.Where(e => e.Id.Equals(input.MenuItemId)).FirstOrDefault();
                AspUserService userService = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
                
                if (menuItem == null)
                {
                    output.Result = "DOES_NOT_EXIST";
                } else
                {
                    if (userService.IsStaff || menuItem.Menu.Vendor.Owner == userService.User)
                    {
                        menuItem.Deleted = true;
                        _db.SaveChanges();
                        output.Result = "OK";
                    }
                    else
                    {
                        output.Result = "NO_PRIVILEGE";
                    }
                }
            } else
            {
                output.Result = "INPUT_IS_NULL";
            }

            return output;
        }

        [HttpPost]
        [Route("Api/MenuItem/RetrieveById")]
        public MenuItemInfoOutput RetrieveMenuItemById([FromBody] MenuItemInfoInput input)
        {
            MenuItemInfoOutput output = new MenuItemInfoOutput();
            if (!string.IsNullOrEmpty(input.MenuItemId))
            {
                MenuItem menuItem = _db.MenuItems.Where(e => e.Id.Equals(input.MenuItemId)).FirstOrDefault();
                AspUserService userService = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (menuItem == null)
                {
                    output.Result = "DOES_NOT_EXIST";
                }
                else
                {
                    if (userService.IsStaff || menuItem.Menu.Vendor.Owner == userService.User)
                    {
                        output.MenuName = menuItem.Name;
                        output.Result = "OK";
                    }
                    else
                    {
                        output.Result = "NO_PRIVILEGE";
                    }
                }
            }
            else
            {
                output.Result = "INPUT_IS_NULL";
            }

            return output;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Api/MenuItem/RetrieveListByMenuId")]
        public MenuItemInfoOutput RetrieveListByMenuId([FromBody] MenuItemInfoInput input)
        {
            MenuItemInfoOutput output = new MenuItemInfoOutput();
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                if (string.IsNullOrEmpty(input.MenuId))
                {
                    output.Result = "INPUT_IS_NULL";
                } else
                {
                    Menu menu = _db.Menus.Where(e => e.Id.Equals(input.MenuId) && e.Deleted == false).FirstOrDefault();
                    if (menu == null)
                    {
                        output.Result = "DOES_NOT_EXIST";
                    } else
                    {
                        List<MenuItem> menuItemList = menu.MenuItems.Where(e => e.Deleted == false).OrderBy(e => e.Name).ToList();
                        List<MenuItemInfo> newMenuItemList = new List<MenuItemInfo>();

                        foreach (MenuItem item in menuItemList)
                        {
                            MenuItemInfo menuItem = new MenuItemInfo()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                ShortDesc = item.ShortDesc,
                                LongDesc = item.LongDesc,
                                Price = item.Price.ToString(),
                                ImgUrl = item.ImgUrl,
                                WaitingTime = item.WaitingTime
                            };
                            newMenuItemList.Add(menuItem);
                        }

                        output.MenuItemList = newMenuItemList;
                        output.MenuName = menu.Name;
                        output.MenuId = menu.Id;
                        output.Result = "OK";
                    }
                }
            }
            return output;
        }
    }
}