using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Services;
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
        private readonly PatientService _patientsService;

        public DoctorDashboardController(PatientService patientsService)
        {
            _patientsService = patientsService;
        }
        
        [Authorize(Roles = "Doctor")]
        public IActionResult Index(string doctorUID)
        {
            List<Patient> patients = _patientsService.selectForDoctorUID(doctorUID);

            return View("/Views/Dashboard/Doctor/DoctorDashboardHome.cshtml");
        }
    }
}
