using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Diagnoses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Models.Accounts.Patient;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CancerRegistry.Services
{
    public class DoctorService
    {
        private readonly DiagnoseContext _diagnoseContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorService(DiagnoseContext diagnoseContext, UserManager<ApplicationUser> userManager) 
        {
            _diagnoseContext = diagnoseContext;
            _userManager = userManager;
        }

        public async Task<Doctor> GetByUserIdAsync(string userId)
        {
            return await _diagnoseContext.Doctors.Where(d => d.UserId == userId).SingleOrDefaultAsync();
        }

        public void Create(Doctor doctor)
        {
            _diagnoseContext.Doctors.Add(doctor);
            _diagnoseContext.SaveChanges();
        }

        public async Task<bool> AddPatient(string firstName, string lastName, string egn,string phoneNumber, DateTime birthDate, string gender)
        {
            var patientAccount = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = egn,
                FirstName = firstName,
                LastName = lastName,
                EGN = egn, 
                PhoneNumber = phoneNumber,
                BirthDate = birthDate,
                Gender = gender
            };

            var patient = new Patient()
            {
                UserId = patientAccount.Id,
                PhoneNumber = Convert.ToInt64(patientAccount.PhoneNumber)
            };

            var temporaryPatientPassword = CreatePatientPassword(firstName, lastName, egn);
            var result = await _userManager.CreateAsync(patientAccount, temporaryPatientPassword);

            if (!result.Succeeded) return false;
            
            await _diagnoseContext.Patients.AddAsync(patient);
            
            return true;
        }

        private string CreatePatientPassword(string firstName, string lastName, string egn)
        {
           return string.Concat(
                char.ToUpper(firstName[0]),
                lastName.First().ToString().ToUpper() + lastName.Substring(1),
                "_",
                egn);
        }
    }
}
