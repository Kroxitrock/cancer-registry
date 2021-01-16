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
    public class DoctorExistingDiagnoseController : Controller
    {
        private readonly DiagnoseService _diagnoseService;

        public DoctorExistingDiagnoseController(DiagnoseService diagnoseService)
        {
            _diagnoseService = diagnoseService;
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult Index(long diagnoseId)
        {
            Diagnose diagnose = _diagnoseService.GetById(diagnoseId);

            return View("/Views/Diagnose/DoctorExistingDiagnoseView.cshtml");
        }
    }
}
