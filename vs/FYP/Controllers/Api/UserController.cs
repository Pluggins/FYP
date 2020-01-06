using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _db = db;
        }

        [HttpPost]
        [Route("Api/User/Create")]
        public async Task<CreateUserOutput> Create([FromBody] CreateUserInput input)
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
                    _db._Users.Add(newUser);
                    _db.SaveChanges();
                    output.Status = "OK";
                } else
                {
                    output.Status = "INTERNAL_ERROR";
                }
            }
            return output;
        }

        [HttpPost]
        [Route("Api/User/CreateSession")]
        public LoginUserOutput Login([FromBody] LoginUserInput input)
        {
            LoginUserOutput output = new LoginUserOutput();
            if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password))
            {
                output.Result = "FIELD_INCOMPLETE";
            } else
            {
                IdentityUser aspUser = _db._AspNetUsers.FirstOrDefault();
                if (aspUser == null)
                {
                    output.Result = "USER_NOT_FOUND";
                } else
                {
                    IdentityUser user = _db._AspNetUsers.Where(e => e.Id.Equals(aspUser.Id)).FirstOrDefault();
                    if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, input.Password) == PasswordVerificationResult.Success)
                    {
                        AppLoginSession newSession = new AppLoginSession(Guid.NewGuid().ToString());
                        newSession.User = _db._Users.Where(e => e.AspNetUser.Equals(user)).FirstOrDefault();
                        newSession.Status = 1;
                        _db.AppLoginSessions.Add(newSession);
                        _db.SaveChanges();
                        output.SessionId = newSession.Id;
                        output.Key = newSession.Key;
                        output.Result = "OK";
                    } else
                    {
                        output.Result = "PASSWORD_MISMATCH";
                    }
                    
                }
            }
            return output;
        }
    }
}