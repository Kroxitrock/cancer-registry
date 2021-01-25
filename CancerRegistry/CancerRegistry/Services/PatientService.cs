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

        public async Task AddPatient(string id, string number)
        {
            await _diagnoseContext.Patients.AddAsync(new Patient() {UserId = id, PhoneNumber = Int64.Parse(number)});
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
                .Include(d=>d.Doctor)
                .Include(d=>d.Patient)
                .SingleOrDefaultAsync();

            if (diagnose == null) return null;
            
            var doctor = await _userManager.FindByIdAsync(diagnose.Doctor.UserId);
            var patient = await _userManager.FindByIdAsync(diagnose.Patient.UserId);

            var tumorState = StateTranslator.TranslateTumorState(diagnose.PrimaryTumor);
            var distantMetastasisState = StateTranslator.TranslateMetastatsisState(diagnose.DistantMetastasis);
            var regionalLymphNodesState = StateTranslator.TranslateRegionalLymphNodesState(diagnose.RegionalLymphNodes);

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

        public async Task<CurrentTreatmentOutputModel> GetCurrentTreatment(string patientId)
        {
            var activeDiagnoseId = await _diagnoseContext.Patients
                .Where(p => p.UserId == patientId)
                .Select(x => x.ActiveDiagnoseId)
                .SingleOrDefaultAsync();

            var treatment = await _diagnoseContext.Diagnoses
                .Where(d => d.Treatment.DiagnoseId == activeDiagnoseId)
                .Include(d=>d.Treatment)
                .Include(d => d.Doctor)
                .Include(d=>d.Patient)
                .SingleOrDefaultAsync();

            if (treatment == null) return null;
            
            var doctor = await _userManager.FindByIdAsync(treatment.Doctor.UserId);
            var patient = await _userManager.FindByIdAsync(treatment.Patient.UserId);

            var model = new CurrentTreatmentOutputModel()
            {
                DoctorName = doctor.FirstName + " " + doctor.LastName,
                PatientName = patient.FirstName + " " + patient.LastName,
                AddedOn = treatment.Treatment.Beginning.ToShortDateString(),
                Treatment = StateTranslator.GetTreatmentDisplayName(treatment.Treatment.Surgery, treatment.Treatment.Radiation, treatment.Treatment.Chemeotherapy, treatment.Treatment.EndocrineTreatment),
                Description = StateTranslator.GetTreatmentDescription(treatment.Treatment.Surgery, treatment.Treatment.Radiation, treatment.Treatment.Chemeotherapy, treatment.Treatment.EndocrineTreatment)
            };

            return model;
        }
    }
}
