using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShowDataWebApp.Models;
using ShowDataWebApp.Repository.IRepository;

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User user)
        {
            var loggedInUser = await _userRepository.LoginAsync(StaticUrlBase.UserApiUrl + "authenticate", user);
            if (loggedInUser != null && loggedInUser.Token != null)
            {
                HttpContext.Session.Set("ShowDataToken", JsonSerializer.SerializeToUtf8Bytes(loggedInUser.Token));
                return RedirectToAction("Index");
            }
            else
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
            bool didRegister = await _userRepository.RegisterAsync(StaticUrlBase.UserApiUrl + "register", user);
            if (didRegister)
            {
                return RedirectToAction("Login");
            }
            else
                return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
