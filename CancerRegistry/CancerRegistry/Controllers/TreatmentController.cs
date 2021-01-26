﻿using CancerRegistry.Identity;
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
        private readonly DiagnoseService _diagnoseService;
        private readonly TreatmentService _treatmentService;

        public TreatmentController(DiagnoseService diagnoseService, TreatmentService treatmentService)
        {
            _diagnoseService = diagnoseService;
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
        public async Task<IActionResult> CreateAsync(long diagnoseId, DateTime end, DiagnosedChemeotherapy Chemeotherapy,
            DiagnosedEndocrineTreatment EndocrineTreatment, DiagnosedRadiation Radiation, DiagnosedSurgery Surgery)
        {

            Diagnose diagnose = await _diagnoseService.GetByIdAsync(diagnoseId);

            diagnose.Treatment = new Treatment()
            {
                Beginning = DateTime.Now,
                End = end,
                Chemeotherapy = Chemeotherapy,
                EndocrineTreatment = EndocrineTreatment,
                Radiation = Radiation,
                Surgery = Surgery
            };

            await _diagnoseService.UpdateAsync();

            return RedirectToAction("", "DoctorDashboard");
        }
    }
}
