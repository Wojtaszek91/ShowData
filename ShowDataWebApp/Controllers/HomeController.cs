using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;
using Microsoft.AspNetCore.Http;

namespace ShowDataWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserAccountRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, IUserAccountRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new User());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            if(user.Password == null)
            {
                TempData["noPass"] = "Check your password please !";
            }
            if(user.Username == null || user.Username.Length < 7)
            {
                TempData["noName"] = "Check your username please !";
            }
            if(TempData["noPass"] != null || TempData["noName"] != null)
            {
                return View(new User());
            }

            var loggedInUser = await _userRepository.LoginAsync(StaticUrlBase.UserApiUrl + "authenticate", user);
            if (loggedInUser != null && loggedInUser.Token != null)
            {
                var cookieIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                cookieIdentity.AddClaim(new Claim(ClaimTypes.Name, loggedInUser.Username));
                cookieIdentity.AddClaim(new Claim(ClaimTypes.Role, loggedInUser.Role));

                var userPrincipal = new ClaimsPrincipal(cookieIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                HttpContext.Session.SetString("ShowDataToken", loggedInUser.Token);
                TempData["loggedIn"] = "Hello " + user.Username;
                return RedirectToAction("Index");
            }
            else
            {
                TempData["loginIssue"] = "Invalid username or password !";
                return View(new User());
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if(user.Username == null || user.Username.Length < 7)
            {
                TempData["nameError"] = "Username must be not less than 7 characters !";
            }
            if(user.Password == null || user.Password.Length < 7)
            {
                TempData["passError"] = "Password must be not less than 7 characters !";
            }
            if (TempData["nameError"] != null || TempData["passError"] != null)
            {
                return View();
            }

            bool didRegister = await _userRepository.RegisterAsync(StaticUrlBase.UserApiUrl + "register", user);
            if (didRegister)
            {
                TempData["registOK"] = user.Username + " has been successfull register. You can now login !";
                return RedirectToAction("Login");
            }
            else
                TempData["registError"] = "An error has occured during registration ! Please try again or contact support !";
                return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Clear();
            TempData["loggedOut"] = "You have logged out ! Come back soon !";
            return RedirectToAction("Index");
        }

    }
}
