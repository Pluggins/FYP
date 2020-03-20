using System;
using System.Collections.Generic;
using System.Globalization;
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
        [Route("Api/User/WebLogin")]
        public async Task<LoginUserOutput> WebLogin([FromBody] LoginUserInput input)
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
            AspUserService userService = new AspUserService(_db, this);
            IdentityUser identityUser = _db._AspNetUsers.Where(e => e.Id.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).FirstOrDefault();

            if (userService.IsStaff)
            {
                bool change = false;
                if (!User.IsInRole("Staff"))
                { 
                    await _userManager.AddToRoleAsync(identityUser, "Staff");
                }

                if (!User.IsInRole("Vendor"))
                {
                    await _userManager.AddToRoleAsync(identityUser, "Vendor");
                    change = true;
                }
                if (change)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(identityUser, true);
                }
            } else if (userService.IsVendor)
            {
                bool change = false;
                if (User.IsInRole("Staff"))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, "Staff");
                    change = true;
                }
                if (!User.IsInRole("Vendor"))
                {
                    await _userManager.AddToRoleAsync(identityUser, "Vendor");
                    change = true;
                }
                if (change)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(identityUser, true);
                }
            } else
            {
                bool change = false;
                if (User.IsInRole("Staff"))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, "Staff");
                    change = true;
                }

                if (User.IsInRole("Vendor"))
                {
                    await _userManager.RemoveFromRoleAsync(identityUser, "Vendor");
                    change = true;
                }

                if (change)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(identityUser, true);
                }
            }

            return true;
        }

        [HttpPost]
        [Route("Api/User/RetrieveTempStore")]
        public bool RetrieveTempStore()
        {
            string id = Request.Cookies["CaptureId"].ToString();
            string code = Request.Cookies["CaptureCode"].ToString();
            AspUserService aspUser = new AspUserService(_db, this);

            if (id != null && code != null && aspUser.IsValid)
            {
                MemberCapture capture = _db.MemberCaptures.Where(e => e.Id.Equals(id.ToString()) && e.Status == 2).FirstOrDefault();
                if (capture != null)
                {
                    if (capture.Code.Equals(code.ToString()))
                    {
                        List<Order> orders = capture.AppLoginSession.User.ListOrders.ToList();
                        foreach (Order item in orders)
                        {
                            item.User = aspUser.User;
                        }
                        Response.Cookies.Append("CaptureId", capture.Id,
                            new Microsoft.AspNetCore.Http.CookieOptions()
                            {
                                Expires = DateTime.UtcNow
                            }); ;
                        Response.Cookies.Append("CaptureCode", capture.Code,
                            new Microsoft.AspNetCore.Http.CookieOptions()
                            {
                                Expires = DateTime.UtcNow
                            });
                        capture.AppLoginSession.User.Status = 0;
                        capture.AppLoginSession.User.Deleted = true;
                        capture.Status = 0;
                        _db.SaveChanges();
                    }
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

        [HttpPost]
        [Route("Api/User/GetDetailBySessionId")]
        public UserInfoOutput GetDetailBySessionId([FromBody] UserInfoInput input)
        {
            UserInfoOutput output = new UserInfoOutput();
            
            if (input == null)
            {
                output.Result = "INPUT_IS_NULL";
            } else
            {
                AppLoginSession session = _db.AppLoginSessions.Where(e => e.Id.Equals(input.SessionId) && e.Status == 1).FirstOrDefault();

                if (session == null)
                {
                    output.Result = "SESSION_NOT_EXIST";
                } else
                {
                    if (session.Key.Equals(input.SessionKey))
                    {
                        User user = session.User;
                        List<Order> orders = user.ListOrders.Where(e => e.Deleted == false).OrderByDescending(e => e.DateCreated).ToList();
                        List<OrderPreviousItem> newOrders = new List<OrderPreviousItem>();
                        NumberFormatInfo nfi = new CultureInfo("ms-MY", false).NumberFormat;
                        nfi.CurrencyDecimalDigits = 2;

                        foreach (Order item in orders)
                        {
                            OrderPreviousItem newItem = new OrderPreviousItem()
                            {
                                OrderId = item.Id,
                                OrderDate = item.DateCreated.ToString(),
                                Price = item.Amount.ToString("C", nfi)
                            };
                            newOrders.Add(newItem);
                        }

                        if (string.IsNullOrEmpty(user.Email))
                        {
                            output.IsMember = false;
                        } else
                        {
                            output.IsMember = true;
                        }
                        output.Orders = newOrders;
                        output.UserEmail = user.Email;
                        output.UserName = user.FName + user.LName;
                        output.DateJoined = user.DateCreated.ToString();
                        output.Result = "OK";
                    } else
                    {
                        output.Result = "CREDENTIAL_ERROR";
                    }
                }
            }

            return output;
        }
    }
}