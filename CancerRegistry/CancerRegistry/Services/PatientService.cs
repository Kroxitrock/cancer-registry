using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class PatientService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public PatientService(DiagnoseContext diagnoseContext)
        {
            _diagnoseContext = diagnoseContext;
        }

        public async Task<Patient> GetByIdAsync(string id) {
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
    }
}
