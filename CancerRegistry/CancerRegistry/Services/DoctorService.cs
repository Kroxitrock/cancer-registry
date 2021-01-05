using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Diagnoses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class DoctorService
    {
        private readonly DiagnoseContext _diagnoseContext;

        public DoctorService(DiagnoseContext diagnoseContext) {
            _diagnoseContext = diagnoseContext;
        }

        public Doctor GetByUserId(string userId)
        {
            return _diagnoseContext.Doctors.Where(d => d.UserId == userId).SingleOrDefault();
        }

        public void Create(Doctor doctor)
        {
            _diagnoseContext.Doctors.Add(doctor);
            _diagnoseContext.SaveChanges();
        }
    }
}
