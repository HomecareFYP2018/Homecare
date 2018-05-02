using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYPHomecare.Models;
using System.Security.Claims;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace FYPHomecare.Controllers
{
    public class AccountController : Controller
    {
        private static string AuthScheme = "UserSecurity";
        private AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["Layout"] = "_Layout";
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginUser user,
                                    string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ClaimsPrincipal principal = null;
            if (SecureValidUser(user.UserId, user.Password, out principal))
            {
                HttpContext.Authentication.SignInAsync(AuthScheme, principal);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Message"] = "Incorrect User ID or Password";
                ViewData["Layout"] = "_Layout";
                return View("Login");
            }
        }

        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.Authentication.SignOutAsync(AuthScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Forbidden(string returnUrl = null)
        {
            ViewData["Layout"] = "_Layout";
            return View();
        }

        private bool SecureValidUser(string uid,
                                     string pw,
                                     out ClaimsPrincipal principal)
        {
            string returnUrl = ViewData["ReturnUrl"] as string;

            // TODO P10 Task 1-1 Use DbSet.FromSql method to retrieve the appUser:
            //AppUser appUser = null; // this line of code need to be modified

            string sql = $"SELECT * FROM Users WHERE nric='{uid}' AND password = HASHBYTES('SHA1','{pw}')";
            DbSet<Users> dbs = _dbContext.Users;
            Users appUser = dbs.FromSql(sql).FirstOrDefault();

            string sql1 = $"SELECT * FROM Service_Provider WHERE nric='{uid}' AND password = HASHBYTES('SHA1','{pw}')";
            DbSet<ServiceProvider> dbS = _dbContext.ServiceProvider;
            ServiceProvider provider = dbS.FromSql(sql1).FirstOrDefault();

            principal = null;
            if (appUser != null)
            {
                principal =
                   new ClaimsPrincipal(
                   new ClaimsIdentity(
                      new Claim[] {
                     new Claim(ClaimTypes.NameIdentifier,appUser.UsersId.ToString()),
                     new Claim(ClaimTypes.Name, appUser.Name),
                     new Claim(ClaimTypes.Role, "User")
                      },
                      "Basic"));
                return true;
            }
            else if(provider != null)
            {
                principal =
                   new ClaimsPrincipal(
                   new ClaimsIdentity(
                      new Claim[] {
                     new Claim(ClaimTypes.NameIdentifier,provider.ServiceProviderId.ToString()),
                     new Claim(ClaimTypes.Name, provider.Name),
                     new Claim(ClaimTypes.Role, "ServiceProvider")
                      },
                      "Basic"));
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}