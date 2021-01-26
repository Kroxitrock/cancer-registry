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
using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Diagnoses.Treatments;

namespace CancerRegistry.Controllers
{
    public class DoctorDashboardController : Controller
    {
        private readonly PatientService _patientsService;
        private readonly DiagnoseService _diagnoseService;
        private readonly HealthCheckService _healthCheckService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly DoctorService _doctorService;
        private readonly TreatmentService _treatmentService;

        public DoctorDashboardController(PatientService patientsService, DiagnoseService diagnoseService,
            HealthCheckService healthCheckService, UserManager<ApplicationUser> userManager, DoctorService doctorService, TreatmentService treatmentService)
        {
            _patientsService = patientsService;
            _diagnoseService = diagnoseService;
            _healthCheckService = healthCheckService;
            _doctorService = doctorService;
            _userManager = userManager;
            _treatmentService = treatmentService;
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult Index()
        {
            List<Patient> patients = _patientsService.SelectForDoctorUIDAsync(_userManager.GetUserId(HttpContext.User)).Result;

            List<DoctorDashboardPatientModel> patientsOutput = new List<DoctorDashboardPatientModel>();

            patients.ForEach(patient =>
            {
                long diagnoseId = -1;
                long treatmentId = -1;
                long healthCheckId = -1;
                if (patient.ActiveDiagnoseId > 0)
                {
                    Diagnose diagnose = _diagnoseService.GetByIdAsync(patient.ActiveDiagnoseId).Result;
                    diagnoseId = diagnose.Id;
                    if (diagnose.Treatment != null)
                    {
                        treatmentId = diagnose.Treatment.Id;
                    }

                    HealthCheck healthCheck = _healthCheckService.getLastForDiagnoseAsync(diagnose.Id).Result;

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

        public async Task<IActionResult> AddDiagnose(string patientId)
        {
            var patient = await _userManager.FindByIdAsync(patientId);

            var model = new DiagnoseModel()
            {
                PatientName = patient.FirstName + " " + patient.LastName,
                PatientId = patientId,
            };

            return View("/Views/Diagnose/AddDiagnose.cshtml", model);
        }

        public async Task<IActionResult> AddTreatment(string patientId)
        {
            long diagnoseId = -1;

            var diagnose = await _diagnoseService.GetActiveDiagnose(patientId);
            
            if(diagnose != null)
                diagnoseId = diagnose.Id;

            var model = new TreatmentModel() { IsDiagnoseExisting = diagnose != null, PatientId = patientId, DiagnoseId = diagnoseId };
            return View("/Views/Treatment/AddTreatment.cshtml", model);
        }

        public async Task<IActionResult> History(string patientId)
        {
            var history = await _patientsService.GetHistory(patientId);
            return View("/Views/Dashboard/Doctor/PatientHistory.cshtml", history);
        }
        
        [HttpGet]
        public IActionResult AddPatient()
        {
            var model = new AddPatientModel();
            return View("/Views/Dashboard/Doctor/AddPatient.cshtml", model);
        }

        public async Task<IActionResult> AllPatients()
        {
            var patients = await _patientsService.GetAllPatients();
            return View("/Views/Dashboard/Doctor/AllPatients.cshtml", patients);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatient(AddPatientModel model)
        {
            if (!ModelState.IsValid)
                return View("/Views/Dashboard/Doctor/AddPatient.cshtml", model);

            var result = await _doctorService.AddPatient(model.FirstName, model.LastName, model.EGN, model.PhoneNumber, model.BirthDate, model.Gender);

            if (result.Succeeded)
                return RedirectToAction("Index");

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err);

            return View("/Views/Dashboard/Doctor/AddPatient.cshtml", model);
        }
    }
}
