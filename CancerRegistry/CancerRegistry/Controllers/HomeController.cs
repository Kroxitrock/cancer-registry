using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CancerRegistry.Models;
using Microsoft.AspNetCore.Authorization;

namespace CancerRegistry.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Info()
        {
            return View();
        }

        public IActionResult PatientDashboard()
            => View("/Views/Dashboard/Patient/PatientDashboardIndex.cshtml");

        [AllowAnonymous]
        public IActionResult DoctorDashboard()
            => View("/Views/Dashboard/Doctor/DoctorDashboardIndex.cshtml");

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
