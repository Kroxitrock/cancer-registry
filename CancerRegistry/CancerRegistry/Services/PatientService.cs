using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Identity.Data;
using CancerRegistry.Models.Diagnoses.Treatments;
using Microsoft.AspNetCore.Identity;

namespace CancerRegistry.Services
{
    public class PatientService
    {
        private readonly DiagnoseContext _diagnoseContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientService(DiagnoseContext diagnoseContext, UserManager<ApplicationUser> userManager)
        {
            _diagnoseContext = diagnoseContext;
            _userManager = userManager;
        }

        public async Task<Patient> GetByIdAsync(string id)
        {
            return await _diagnoseContext.Patients
                .Where(p => p.UserId == id)
                .SingleOrDefaultAsync();
        }

        public async Task<List<Patient>> SelectForDoctorUIDAsync(string doctorUID)
        {
            return await _diagnoseContext.Doctors
                .Where(doctor => doctor.UserId == doctorUID)
                .Join(_diagnoseContext.Diagnoses,
                    doctor => doctor.UserId,
                    diagnose => diagnose.Doctor.UserId,
                    (doctor, diagnose) => diagnose)
                .Join(_diagnoseContext.Patients,
                    diagnose => diagnose.Id,
                    patient => patient.ActiveDiagnoseId,
                    (diagnose, patient) => patient)
                .ToListAsync();
        }

        internal async Task UpdateAsync()
        {
            await _diagnoseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllPatients()
        {
            var patients = await _userManager.GetUsersInRoleAsync("Patient");
            return patients;
        }
        
        public async Task<CurrentDiagnoseOutputModel> GetActiveDiagnose(string patientId)
        {
            var p = await _diagnoseContext.Patients
                .Where(x => x.UserId == patientId)
                .SingleOrDefaultAsync();

            var diagnose = await _diagnoseContext.Diagnoses
                .Where(d => d.Id == p.ActiveDiagnoseId)
                .SingleOrDefaultAsync();

            if (diagnose == null) return null;
            
            var doctor = await _userManager.FindByIdAsync(diagnose.Doctor.UserId);
            var patient = await _userManager.FindByIdAsync(diagnose.Patient.UserId);

            var tumorState = TranslateTumorState(diagnose.PrimaryTumor);
            var distantMetastasisState = TranslateMetastatsisState(diagnose.DistantMetastasis);
            var regionalLymphNodesState = TranslateRegionalLymphNodesState(diagnose.RegionalLymphNodes);

            var outputModel = new CurrentDiagnoseOutputModel()
            {
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                PatientName = patient.FirstName + " " + patient.LastName,
                Stage = diagnose.Stage.ToString(),
                PrimaryTumorState = tumorState,
                DistantMetastasisState = distantMetastasisState,
                RegionalLymphNodesState = regionalLymphNodesState
            };
            
            return outputModel;
        }
        public async Task GetCurrentTreatment(string patientId)
        {
            var activeDiagnoseId = await _diagnoseContext.Patients
                .Where(p => p.UserId == patientId)
                .Select(x=> x.ActiveDiagnoseId)
                .SingleOrDefaultAsync();

            var treatment = await _diagnoseContext.Treatments
                .Where(t => t.DiagnoseId == activeDiagnoseId)
                .SingleOrDefaultAsync();

            var doctor = await _userManager.FindByIdAsync(treatment.Diagnose.Doctor.UserId);
            var patient = await _userManager.FindByIdAsync(treatment.Diagnose.Patient.UserId);

            var model = new CurrentTreatmentOutputModel()
            {
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                PatientName = patient.FirstName + " " + patient.LastName,
                AddedOn = treatment.Beginning.ToShortDateString(),
                Description = ""
            };
        }
        
        
        private string TranslateTumorState(PrimaryTumorState state)
        {
            var primaryTumorState = "";
            if (state == PrimaryTumorState.T0) primaryTumorState += "T0";
            else if (state == PrimaryTumorState.T1) primaryTumorState += "T1";
            else if (state == PrimaryTumorState.T2) primaryTumorState += "T2";
            else if (state == PrimaryTumorState.T3) primaryTumorState += "T3";
            else if (state == PrimaryTumorState.T4) primaryTumorState += "T4";

            return primaryTumorState;
        }
        private string TranslateMetastatsisState(DistantMetastasisState state)
        {
            var distantMetastasisState = "";
            if (state == DistantMetastasisState.M0) distantMetastasisState += "M0";
            else if (state == DistantMetastasisState.M1) distantMetastasisState += "M2";

            return distantMetastasisState;
        }
        private string TranslateRegionalLymphNodesState(RegionalLymphNodesState state)
        {
            var lymphNodesState = "";
            if (state == RegionalLymphNodesState.N0) lymphNodesState += "N0";
            else if (state == RegionalLymphNodesState.N1) lymphNodesState += "N1";
            else if (state == RegionalLymphNodesState.N2) lymphNodesState += "N2";
            else if (state == RegionalLymphNodesState.N3) lymphNodesState += "N3";

            return lymphNodesState;
        }

        private string TranslateSurgery(DiagnosedSurgery state)
        {
            var surgeryState = "";
            if (state == DiagnosedSurgery.S1) surgeryState += "S1";
            else if (state == DiagnosedSurgery.S2) surgeryState += "S2";
            else if (state == DiagnosedSurgery.S3) surgeryState += "S3";
            else if (state == DiagnosedSurgery.S4) surgeryState += "S4";

            return surgeryState;
        }
        private string TranslateRadiation(DiagnosedRadiation state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedRadiation.R1) endocrineTreatmentState += "E1";
            else if (state == DiagnosedRadiation.R2) endocrineTreatmentState += "E2";
            else if (state == DiagnosedRadiation.R3) endocrineTreatmentState += "E3";
            else if (state == DiagnosedRadiation.R4) endocrineTreatmentState += "E4";

            return endocrineTreatmentState;
        }

        private string TranslateChemeotherapy(DiagnosedChemeotherapy state)
        {
            var chemeotherapyState = "";
            if (state == DiagnosedChemeotherapy.C1) chemeotherapyState += "C1";
            else if (state == DiagnosedChemeotherapy.C2) chemeotherapyState += "C2";
            else if (state == DiagnosedChemeotherapy.C3) chemeotherapyState += "C3";
            else if (state == DiagnosedChemeotherapy.C4) chemeotherapyState += "C4";

            return chemeotherapyState;
        }

        private string TranslateEndocrineTreatment(DiagnosedEndocrineTreatment state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedEndocrineTreatment.E1) endocrineTreatmentState += "E1";
            else if (state == DiagnosedEndocrineTreatment.E2) endocrineTreatmentState += "E2";
            else if (state == DiagnosedEndocrineTreatment.E3) endocrineTreatmentState += "E3";
            else if (state == DiagnosedEndocrineTreatment.E4) endocrineTreatmentState += "E4";

            return endocrineTreatmentState;
        }
        
    }
}
