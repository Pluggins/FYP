using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class ItemMenuApiController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ItemMenuApiController(
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
    }
}