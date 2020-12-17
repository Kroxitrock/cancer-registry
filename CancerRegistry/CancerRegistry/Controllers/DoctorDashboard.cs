using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Controllers
{
    public class DoctorDashboard : Controller
    {
        public IActionResult Home()
            => View("/Views/Dashboard/Doctor/DoctorDashboardHome.cshtml");
    }
}
