using CancerRegistry.Identity;
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
    public class DoctorDashboardController : Controller
    {
        private readonly PatientService _patientsService;
        private readonly DiagnoseService _diagnoseService;
        private readonly HealthCheckService _healthCheckService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorDashboardController(PatientService patientsService, DiagnoseService diagnoseService,
            HealthCheckService healthCheckService, UserManager<ApplicationUser> userManager)
        {
            _patientsService = patientsService;
            _diagnoseService = diagnoseService;
            _healthCheckService = healthCheckService;
            _userManager = userManager;
        }
        
        [Authorize(Roles = "Doctor")]
        public IActionResult Index()
        {
            List<Patient> patients = _patientsService.SelectForDoctorUID(_userManager.GetUserId(HttpContext.User));

            List<DoctorDashboardPatientModel> patientsOutput = new List<DoctorDashboardPatientModel>();

            patients.ForEach(patient =>
            {
                long diagnoseId = -1;
                long treatmentId = -1;
                long healthCheckId = -1;
                if (patient.ActiveDiagnoseId > 0)
                {
                    Diagnose diagnose = _diagnoseService.GetById(patient.ActiveDiagnoseId);
                    diagnoseId = diagnose.Id;
                    if (diagnose.Treatment != null)
                    {
                        treatmentId = diagnose.Treatment.Id;
                    }

                    HealthCheck healthCheck = _healthCheckService.getLastForDiagnose(diagnose.Id);

                    if (healthCheck != null)
                    {
                        healthCheckId = healthCheck.Id;
                    }
                }


                ApplicationUser patientPrincipal = _userManager.FindByIdAsync(patient.UserId).Result;
                patientsOutput.Add(new DoctorDashboardPatientModel()
                {
                    Id = patient.UserId,
                    PhoneNumber = patient.PhoneNumber,
                    DiagnoseId = diagnoseId,
                    TreatmentId = treatmentId,
                    LastHealthCheckId = healthCheckId,
                    Name = patientPrincipal.FirstName + " " + patientPrincipal.LastName
                });
            });
            

            return View("/Views/Dashboard/Doctor/DoctorDashboardHome.cshtml", patientsOutput);
        }
    }
}
