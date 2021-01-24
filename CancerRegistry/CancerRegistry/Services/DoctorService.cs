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

        public async Task<OperationResult> AddPatient(string firstName, string lastName, string egn,string phoneNumber, DateTime birthDate, string gender)
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

            var temporaryPatientPassword = CreatePatientPassword(egn);
            var result = await _userManager.CreateAsync(patientAccount, temporaryPatientPassword);
            
            if (!result.Succeeded) return AddPatientResult(result);
            
            var roleResult = await _userManager.AddToRoleAsync(patientAccount, "Patient");
            
            await _diagnoseContext.Patients.AddAsync(patient);
            await _diagnoseContext.SaveChangesAsync();
            
            return new OperationResult();
        }

        private string CreatePatientPassword(string egn) 
            => string.Concat("Patient", "_", egn);

        private OperationResult AddPatientResult(IdentityResult result)
        {
            var addPatientResult = new OperationResult();

            addPatientResult.Succeeded = false;
            addPatientResult.Errors = new List<string>();
            foreach (var err in result.Errors)
                addPatientResult.Errors.Add(err.Description);

            return addPatientResult;
        }

    }
}
