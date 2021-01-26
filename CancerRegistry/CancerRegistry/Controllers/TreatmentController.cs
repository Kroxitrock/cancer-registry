using CancerRegistry.Identity;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.Treatments;
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
    public class TreatmentController : Controller
    {
        private readonly TreatmentService _treatmentService;

        public TreatmentController(TreatmentService treatmentService)
        {
            _treatmentService = treatmentService;
        }

        [Authorize(Roles = "Doctor")]
        public IActionResult Index(long diagnoseId, long treatmentId, string patientId, string patientName)
        {
            TreatmentModel treatmentModel;

            if (treatmentId == -1)
            {
                treatmentModel = new TreatmentModel()
                {
                    IsExisting = false,
                    IsDiagnoseExisting = true,
                    DiagnoseId = diagnoseId,
                    PatientId = patientId,
                    PatientName = patientName
                };
            }
            else
            {
                Treatment treatment = _treatmentService.GetByIdAsync(treatmentId).Result;

                treatmentModel = new TreatmentModel()
                {
                    Id = treatmentId,
                    IsExisting = true,
                    IsDiagnoseExisting = true,
                    DiagnoseId = diagnoseId,
                    PatientId = patientId,
                    PatientName = patientName,
                    End = treatment.End,
                    Chemeotherapy = treatment.Chemeotherapy,
                    EndocrineTreatment = treatment.EndocrineTreatment,
                    Radiation = treatment.Radiation,
                    Surgery = treatment.Surgery
                };
            }

            return View("/Views/Treatment/TreatmentView.cshtml", treatmentModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync(TreatmentModel model)
        {
            if (!ModelState.IsValid)
                return View("/Views/Treatment/AddTreatment.cshtml",model);
            
            await _treatmentService.AddTreatmentToDiagnose(model);

            return RedirectToAction("", "DoctorDashboard");
        }
    }
}
