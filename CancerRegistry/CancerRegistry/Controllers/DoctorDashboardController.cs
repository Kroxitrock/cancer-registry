using CancerRegistry.Models.Diagnoses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Controllers
{
    public class DoctorDashboardController : Controller
    {
        private readonly DiagnoseContext _diagnoseContext;

        public DoctorDashboardController(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public IActionResult Home()
            => View("/Views/Dashboard/Doctor/DoctorDashboardHome.cshtml");
        
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("/Views/Dashboard/Doctor/DoctorDashboardHome.cshtml");
        }
    }
}
