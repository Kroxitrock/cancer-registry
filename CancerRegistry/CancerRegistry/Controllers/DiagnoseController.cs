using CancerRegistry.Identity;
using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.HealthChecks;
using CancerRegistry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Controllers
{
    public class DiagnoseController : Controller
    {
        private readonly DiagnoseService _diagnoseService;
        private readonly HealthCheckService _healthCheckService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DiagnoseController(DiagnoseService diagnoseService, HealthCheckService healthCheckService, DoctorService doctorService, PatientService patientService, UserManager<ApplicationUser> userManager)
        {
            _diagnoseService = diagnoseService;
            _healthCheckService = healthCheckService;
            _doctorService = doctorService;
            _patientService = patientService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult Index(long diagnoseId, string patientId, string patientName)
        {
            
            DiagnoseModel diagnoseModel;
            if (diagnoseId != -1)
            {
                Diagnose diagnose = _diagnoseService.GetByIdAsync(diagnoseId).Result;
                
                diagnoseModel = new DiagnoseModel
                {
                    Id = diagnose.Id,
                    DiagnoseExists = true,
                    Stage = diagnose.Stage,
                    DistantMetastasis = diagnose.DistantMetastasis,
                    PrimaryTumor = diagnose.PrimaryTumor,
                    RegionalLymphNodes = diagnose.RegionalLymphNodes,
                    PatientId = patientId,
                    PatientName = patientName
                };
            } else
            {
                diagnoseModel = new DiagnoseModel
                {
                    DiagnoseExists = false,
                    PatientId = patientId,
                    PatientName = patientName
                };
            }

            return View("/Views/Diagnose/DoctorExistingDiagnoseView.cshtml", diagnoseModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(string patientId, PrimaryTumorState primaryTumor, DistantMetastasisState distantMetastasis, RegionalLymphNodesState regionalLymphNodes)
        {
            Patient patient = _patientService.GetByIdAsync(patientId).Result;
            Doctor doctor = _doctorService.GetByUserIdAsync(_userManager.GetUserId(HttpContext.User)).Result;

            Diagnose diagnose = new Diagnose()
            {
                Patient = patient,
                Doctor = doctor,
                DistantMetastasis = distantMetastasis,
                PrimaryTumor = primaryTumor,
                RegionalLymphNodes = regionalLymphNodes,
                Stage = _diagnoseService.DetermineStage(distantMetastasis, primaryTumor, regionalLymphNodes)
            };

            HealthCheck healthCheck = new HealthCheck()
            {
                Diagnose = diagnose,
                Timestamp = DateTime.Now
            };

            int healthCheckId = _healthCheckService.CreateAsync(healthCheck).Result;

            healthCheck = _healthCheckService.GetByIdAsync(healthCheckId).Result;

            patient.ActiveDiagnoseId = healthCheck.Diagnose.Id;

            await _patientService.UpdateAsync();

            return RedirectToAction("", "DoctorDashboard");
        }
    }
}
