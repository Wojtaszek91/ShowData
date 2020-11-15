using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShowDataWebApp.Controllers
{
    public class SensorController : Controller
    {
        public IActionResult Index()
        {
            string elo = "123";

            ViewData[elo] = "1234";
            return View();
        }
    }
}
