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

        public async Task<CurrentDiagnoseOutputModel> GetActiveDiagnose(string patientId)
        {
            var p = await _diagnoseContext.Patients
                .Where(x => x.UserId == patientId)
                .SingleOrDefaultAsync();

            var diagnose = await _diagnoseContext.Diagnoses
                .Where(d => d.Id == p.ActiveDiagnoseId)
                .Include(d=> d.Doctor)
                .Include(d => d.Patient)
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
            else if (state == PrimaryTumorState.T1) primaryTumorState += "T1";
            else if (state == PrimaryTumorState.T2) primaryTumorState += "T2";
            else if (state == PrimaryTumorState.T3) primaryTumorState += "T3";
            else if (state == PrimaryTumorState.T4) primaryTumorState += "T4";

            return primaryTumorState;
        }
        private string TranslateMetastatsisState(DistantMetastasisState state)
        {
            var primaryTumorState = "";
            if (state == DistantMetastasisState.M0) primaryTumorState += "M0";
            else if (state == DistantMetastasisState.M1) primaryTumorState += "M2";

            return primaryTumorState;
        }
        private string TranslateRegionalLymphNodesState(RegionalLymphNodesState state)
        {
            var primaryTumorState = "";
            if (state == RegionalLymphNodesState.N0) primaryTumorState += "N0";
            else if (state == RegionalLymphNodesState.N1) primaryTumorState += "N1";
            else if (state == RegionalLymphNodesState.N2) primaryTumorState += "N2";
            else if (state == RegionalLymphNodesState.N3) primaryTumorState += "N3";

            return primaryTumorState;
        }
    }
}
