using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Controllers
{
    public class PatientDashboard : Controller
    {
        public IActionResult Home()
            => View("/Views/Dashboard/Patient/PatientDashboardHome.cshtml");
    }
}
