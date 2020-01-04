using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public UserController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        [HttpPost]
        [Route("Api/User/Create")]
        public async Task<CreateUserOutput> CreateUser(CreateUserInput input)
        {
            CreateUserOutput output = new CreateUserOutput();
            if (string.IsNullOrEmpty(input.Email))
            {
                output.Status = "EMAIL_IS_NULL";
            } else if (string.IsNullOrEmpty(input.Password))
            {
                output.Status = "PASSWORD_IS_NULL";
            } else if (input.Password.Length < 6)
            {
                output.Status = "PASSWORD_LENGTH_TOO_SHORT";
            } else if (!input.Password.Equals(input.ConfirmPassword)) 
            {
                output.Status = "PASSWORD_NOT_MATCH";
            } else
            {
                IdentityUser newAspUser = new IdentityUser()
                {
                    UserName = input.Email,
                    Email = input.Email
                };
                var status = await _userManager.CreateAsync(newAspUser, input.Password);
                if (status.Succeeded)
                {
                    User newUser = new User()
                    {
                        FName = input.FName,
                        LName = input.LName,
                        Email = input.Email,
                        AspNetUser = newAspUser
                    };
                    _db.ApplicationUsers.Add(newUser);
                    _db.SaveChanges();
                    output.Status = "OK";
                } else
                {
                    output.Status = "INTERNAL_ERROR";
                }
            }
            return output;
        }
    }
}