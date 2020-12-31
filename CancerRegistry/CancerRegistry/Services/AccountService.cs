using CancerRegistry.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return false;

            await _signInManager.SignOutAsync();

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);

            return signInResult.Succeeded;
        }

        public async Task LogoutUser()
         => await _signInManager.SignOutAsync();

        public async Task<RegistrationResult> RegisterPatient(
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            string password)
        {
            RegistrationResult registrationResult = new RegistrationResult();
            
            ApplicationUser user = new ApplicationUser()
            {
                UserName = egn,
                FirstName = firstName,
                LastName = lastName,
                EGN = egn,
                PhoneNumber = phoneNumber
            };

            var registerResult = await _userManager.CreateAsync(user, password);
            var roleResult = await _userManager.AddToRoleAsync(user,"Patient");

            if (registerResult.Succeeded && roleResult.Succeeded) 
                return registrationResult;

            registrationResult.Succeeded = false;
            registrationResult.Errors = new List<string>();
            foreach (var err in registerResult.Errors)
                registrationResult.Errors.Add(err.Description);

            foreach (var err in roleResult.Errors)
                registrationResult.Errors.Add(err.Description);

            registrationResult.Errors = registrationResult.Errors.Distinct().ToList();
            return registrationResult;
        }

        public async Task<ApplicationUser> GetPatient(string id)
        {
            var patient = await _userManager.FindByIdAsync(id);
            return patient;
        }

        public async Task<ApplicationUser> GetDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);
            return doctor;
        }
        public async Task<bool> Edit(string id, string firstName, string lastName, string egn, string phoneNumber, DateTime birthDate, string gender)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            user.FirstName = firstName;
            user.LastName = lastName;
            user.EGN = egn;
            user.PhoneNumber = phoneNumber;
            user.UserName = egn;
            user.BirthDate = birthDate;
            user.Gender = gender;

            var result = await _userManager.UpdateAsync(user);
            
            return result.Succeeded;
        }

    }
}
