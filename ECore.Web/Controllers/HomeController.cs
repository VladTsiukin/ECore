using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ECore.Web.Models;
using Microsoft.Extensions.Logging;

namespace ECore.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "О проекте";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Контакты";
            return View();
        }

        [Route("Error")]
        public IActionResult Error()
        {
            _logger.LogError("[<<< ERROR >>>] Request ID: {0}", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
            return View();
        }

    }
}
