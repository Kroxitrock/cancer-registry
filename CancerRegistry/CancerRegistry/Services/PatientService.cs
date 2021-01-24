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

        
        private string TranslateTumorState(PrimaryTumorState state)
        {
            var primaryTumorState = "";
            if (state == PrimaryTumorState.T0) primaryTumorState += "T0";
            else if (state == PrimaryTumorState.T1) primaryTumorState += "T1 = tumor size ≤20 mm";
            else if (state == PrimaryTumorState.T2) primaryTumorState += "T2 = 20 mm but ≤50 mm";
            else if (state == PrimaryTumorState.T3) primaryTumorState += "T3 = 50 mm";
            else if (state == PrimaryTumorState.T4) primaryTumorState += "T4 = tumor of any size with direct extension to the chest wall and/or skin";

            return primaryTumorState;
        }
        private string TranslateMetastatsisState(DistantMetastasisState state)
        {
            var distantMetastasisState = "";
            if (state == DistantMetastasisState.M0) distantMetastasisState += "M0 no evidence of distant metastases";
            else if (state == DistantMetastasisState.M1) distantMetastasisState += "M2 distant detectable metastases as determined by clinical and radiographic means";

            return distantMetastasisState;
        }
        private string TranslateRegionalLymphNodesState(RegionalLymphNodesState state)
        {
            var lymphNodesState = "";
            if (state == RegionalLymphNodesState.N0) lymphNodesState += "N0 no regional lymph node metastases";
            else if (state == RegionalLymphNodesState.N1) lymphNodesState += "N1 metastases to moveable ipsilateral axillary lymph nodes";
            else if (state == RegionalLymphNodesState.N2) lymphNodesState += "N2 metastases in ipsilateral axillary lymph nodes that are clinically fixed";
            else if (state == RegionalLymphNodesState.N3) lymphNodesState += "N3 metastases that are more extensive";

            return lymphNodesState;
        }

        private string TranslateSurgery(DiagnosedSurgery state)
        {
            var surgeryState = "";
            if (state == DiagnosedSurgery.S1) surgeryState += "S1 Total mastectomy ± sentinel node biopsy ± reconstruction; or lumpectomy without lymph node surgery.";
            else if (state == DiagnosedSurgery.S2) surgeryState += "S2 Total mastectomy or lumpectomy + axillary staging ± breast reconstruction.";
            else if (state == DiagnosedSurgery.S3) surgeryState += "S3 If response to pre-operative therapy, total mastectomy or lumpectomy + axillary dissection ± delayed breast reconstruction.";
            else if (state == DiagnosedSurgery.S4) surgeryState += "S4 None";

            return surgeryState;
        }
        private string TranslateRadiation(DiagnosedRadiation state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedRadiation.R1) endocrineTreatmentState += "R1 Whole breast radiation may be added to lumpectomy.";
            else if (state == DiagnosedRadiation.R2) endocrineTreatmentState += "R2 Radiation to whole breast and lymph nodes if involved; follows chemotherapy if provided.";
            else if (state == DiagnosedRadiation.R3) endocrineTreatmentState += "R3 Radiation to chest wall and lymph nodes.";
            else if (state == DiagnosedRadiation.R4) endocrineTreatmentState += "R4 Selective radiation to bone or brain metastases.";

            return endocrineTreatmentState;
        }

        private string TranslateChemeotherapy(DiagnosedChemeotherapy state)
        {
            var chemeotherapyState = "";
            if (state == DiagnosedChemeotherapy.C1) chemeotherapyState += "C1 None";
            else if (state == DiagnosedChemeotherapy.C2) chemeotherapyState += "C2 Systemic adjuvant therapy as indicated by ER, PR, and HER2 status and predictive tests for chemotherapy benefit.";
            else if (state == DiagnosedChemeotherapy.C3) chemeotherapyState += "C3 Pre-operative systemic therapy";
            else if (state == DiagnosedChemeotherapy.C4) chemeotherapyState += "C4 If bone disease present, denosumab, zoledronic acid, or pamidronate.";

            return chemeotherapyState;
        }

        private string TranslateEndocrineTreatment(DiagnosedEndocrineTreatment state)
        {
            var endocrineTreatmentState = "";
            if (state == DiagnosedEndocrineTreatment.E1) endocrineTreatmentState += "E1 If ER-positive, consider tamoxifen for 5 years for prevention.";
            else if (state == DiagnosedEndocrineTreatment.E2) endocrineTreatmentState += "E2 If ER-positive, tamoxifen for 10 years or aromatase inhibitor for 5 years (if post-menopausal only) or switching strategy of tamoxifen/aromatase inhibitor.";
            else if (state == DiagnosedEndocrineTreatment.E3) endocrineTreatmentState += "E3 If ER positive, consider ovarian ablation/ suppression for premenopausal women";
            else if (state == DiagnosedEndocrineTreatment.E4) endocrineTreatmentState += "E4 Treatment regimen based on receptor status";

            return endocrineTreatmentState;
        }
        
    }
}
