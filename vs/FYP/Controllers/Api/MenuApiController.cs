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
                Vendor vendor = _db.Vendors.Where(e => e.Id.Equals(input.VendorId)).FirstOrDefault();
                if (vendor == null)
                {
                    output.Status = "VENDOR_NOT_FOUND";
                } else
                {
                    List<Menu> menuList = vendor.Menus.ToList();
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
                List<Menu> menus = _db.Menus.ToList();
                foreach(Menu item in menus)
                {
                    MenuItemOutput sOutput = new MenuItemOutput()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        ShortDesc = item.ShortDesc
                    };
                    items.Add(sOutput);
                }
                output.Status = "OK";
                output.MenuItems = items;
            }
            return output;
        }
    }
}