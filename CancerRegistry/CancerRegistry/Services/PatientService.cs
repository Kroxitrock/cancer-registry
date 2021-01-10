using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
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

        public List<Patient> SelectForDoctorUID(string doctorUID)
        {
            return _diagnoseContext.Doctors
                .Where(doctor => doctor.UserId == doctorUID)
                .Join(_diagnoseContext.Diagnoses,
                    doctor => doctor.UserId,
                    diagnose => diagnose.Doctor.UserId,
                    (doctor, diagnose) => diagnose)
                .Join(_diagnoseContext.Patients,
                    diagnose => diagnose.Id,
                    patient => patient.ActiveDiagnoseId,
                    (diagnose, patient) => patient)
                .ToList();
        }
    }
}
