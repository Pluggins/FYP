using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FYP.Data;
using FYP.Models;
using FYP.Models.ViewModels;
using FYP.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers.Api
{
    public class UserApiController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public UserApiController(
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
                User user = _db._Users.Where(e => e.Email.ToUpper().Equals(input.Email.ToUpper())).FirstOrDefault();
                
                if (user != null)
                {
                    output.Status = "EMAIL_IN_USE";
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
                            AspNetUser = newAspUser,
                            Status = 1
                        };
                        _db._Users.Add(newUser);
                        _db.SaveChanges();
                        output.Status = "OK";
                    }
                    else
                    {
                        output.Status = "INTERNAL_ERROR";
                    }
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
                IdentityUser aspUser = _db._AspNetUsers.Where(e => e.UserName.ToLower().Equals(input.Email.ToLower())).FirstOrDefault();
                if (aspUser == null)
                {
                    output.Result = "USER_NOT_FOUND";
                } else
                {
                    if (_userManager.PasswordHasher.VerifyHashedPassword(aspUser, aspUser.PasswordHash, input.Password) == PasswordVerificationResult.Success)
                    {
                        AppLoginSession newSession = new AppLoginSession(Guid.NewGuid().ToString(), Request);
                        newSession.User = _db._Users.Where(e => e.AspNetUser.Equals(aspUser)).FirstOrDefault();
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

        [HttpPost]
        [Route("Api/User/AdminLogin")]
        public async Task<LoginUserOutput> AdminLogin([FromBody] LoginUserInput input)
        {
            LoginUserOutput output = new LoginUserOutput();
            if (string.IsNullOrEmpty(input.Email) || string.IsNullOrEmpty(input.Password))
            {
                output.Result = "FIELD_INCOMPLETE";
            }
            else
            {
                IdentityUser aspUser = _db._AspNetUsers.Where(e => e.UserName.ToLower().Equals(input.Email.ToLower())).FirstOrDefault();
                if (aspUser == null)
                {
                    output.Result = "USER_NOT_FOUND";
                }
                else
                {
                    if (_userManager.PasswordHasher.VerifyHashedPassword(aspUser, aspUser.PasswordHash, input.Password) == PasswordVerificationResult.Success)
                    {
                        await _signInManager.SignInAsync(aspUser, true);
                        AspUserService userService = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
                        if (userService.IsStaff)
                        {
                            if (!User.IsInRole("Staff"))
                            {
                                IdentityUser identityUser = _db._AspNetUsers.Where(e => e.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
                                await _userManager.AddToRoleAsync(identityUser, "Staff");
                            }
                        } else
                        {
                            if (User.IsInRole("Staff"))
                            {
                                IdentityUser identityUser = _db._AspNetUsers.Where(e => e.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
                                await _userManager.RemoveFromRoleAsync(identityUser, "Staff");
                            }
                        }
                        output.Result = "OK";
                    }
                    else
                    {
                        output.Result = "PASSWORD_MISMATCH";
                    }

                }
            }
            return output;
        }

        [HttpPost]
        [Route("Api/User/CheckStaffRole")]
        public async Task<bool> CheckStaffRole()
        {
            AspUserService userService = new AspUserService(_db, User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userService.IsStaff)
            {
                if (!User.IsInRole("Staff"))
                {
                    IdentityUser identityUser = _db._AspNetUsers.Where(e => e.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
                    await _userManager.AddToRoleAsync(identityUser, "Staff");
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(identityUser, true);
                }
            }
            else
            {
                if (User.IsInRole("Staff"))
                {
                    IdentityUser identityUser = _db._AspNetUsers.Where(e => e.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();
                    await _userManager.RemoveFromRoleAsync(identityUser, "Staff");
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(identityUser, true);
                }
            }

            return true;
        }

        [HttpPost]
        [Route("Api/User/Logout")]
        public async Task<LoginUserOutput> Logout()
        {
            await _signInManager.SignOutAsync();
            LoginUserOutput output = new LoginUserOutput()
            {
                Result = "OK"
            };

            return output;
        }

        [HttpPost]
        [Route("Api/User/CreateTempUserSession")]
        public CreateUserOutput CreateTempUser()
        {
            CreateUserOutput output = new CreateUserOutput();
            User newUser = new User();
            _db._Users.Add(newUser);
            AppLoginSession loginSession = new AppLoginSession(Guid.NewGuid().ToString(), Request);
            loginSession.User = newUser;
            loginSession.Status = 1;
            _db.AppLoginSessions.Add(loginSession);
            _db.SaveChanges();

            output.UserId = newUser.Id;
            output.SessionId = loginSession.Id;
            output.SessionKey = loginSession.Key;

            return output;
        }
    }
}