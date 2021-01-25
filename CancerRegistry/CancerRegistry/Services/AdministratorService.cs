﻿using CancerRegistry.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Services
{
    public class AdministratorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdministratorService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllPatients()
        {
            var patients = await _userManager.GetUsersInRoleAsync("Patient");
            return patients;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllDoctors()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            return doctors;
        }

        public async Task Logout()
            => await _signInManager.SignOutAsync();

        public async Task<bool> LoginAdmin(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var isAdmin = await _userManager.IsInRoleAsync(user, "Administrator");
            if (!isAdmin) return false;

            await _signInManager.SignOutAsync();

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
            return signInResult.Succeeded;
        }

        public async Task<OperationResult> DeleteUser(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            if (user == null) return UserNotFoundResult();

            var result = await _userManager.DeleteAsync(user);
            return DeleteUserResult(result);
        }

        public async Task<OperationResult> RegisterDoctor(
            string firstName,
            string lastName,
            string egn,
            string uid,
            string gender,
            string bulstat)
        {
            var doctorPassword = string.Concat("Doctor","_", egn);

            ApplicationUser appUser = new ApplicationUser()
            {
                UserName = uid,
                FirstName = firstName,
                LastName = lastName,
                EGN = egn,
                UID = uid,
                Gender = gender,
                HospitalBulstat = bulstat
            };

            var registerResult = await _userManager.CreateAsync(appUser, doctorPassword);

            if (!registerResult.Succeeded)
                return RegistrationResult(registerResult);
            
            var roleResult = await _userManager.AddToRoleAsync(appUser, "Doctor");

            return new OperationResult();
        }

        public async Task<ApplicationUser> GetUserByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<bool> IsPatient(string id)
        {
            var patient = await _userManager.FindByIdAsync(id);
            var result = await _userManager.IsInRoleAsync(patient, "Patient");

            return result;
        }

        public async Task<bool> IsDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);
            var result = await _userManager.IsInRoleAsync(doctor, "Doctor");

            return result;
        }

        #region Private methods

        private OperationResult UserNotFoundResult()
        {
            var operationResult = new OperationResult();
            operationResult.Succeeded = false;
            operationResult.Errors = new List<string>();
            operationResult.Errors.Add("Потребителят не съществува.");
            return operationResult;
        }

        private static OperationResult DeleteUserResult(IdentityResult result)
        {
            OperationResult oResult = new OperationResult();
            if (result.Succeeded)
                return oResult;

            oResult.Succeeded = false;
            oResult.Errors = new List<string>();

            foreach (var err in result.Errors)
                oResult.Errors.Add(err.Description);

            return oResult;
        }
        private OperationResult RegistrationResult(IdentityResult registerResult)
        {
            var registrationResult = new OperationResult();
            
            registrationResult.Succeeded = false;
            registrationResult.Errors = new List<string>();
            foreach (var error in registerResult.Errors)
                registrationResult.Errors.Add(error.Description);
            
            return registrationResult;
        }

        #endregion
    }
}
