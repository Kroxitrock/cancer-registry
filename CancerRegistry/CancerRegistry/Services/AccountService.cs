using CancerRegistry.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<OperationResult> RegisterPatient(
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            string password)
        {
            var user = new ApplicationUser()
            {
                UserName = egn,
                FirstName = firstName,
                LastName = lastName,
                EGN = egn,
                PhoneNumber = phoneNumber
            };

            var registerResult = await _userManager.CreateAsync(user, password);

            if (!registerResult.Succeeded)
                return RegistrationResult(registerResult);
            
            var roleResult = await _userManager.AddToRoleAsync(user,"Patient");

            return new OperationResult();
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
        
        public async Task<OperationResult> EditPatient(string id, string firstName, string lastName, string egn, string phoneNumber, DateTime birthDate, string gender)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) return UserNotFoundResult();

            user.FirstName = firstName;
            user.LastName = lastName;
            user.EGN = egn;
            user.PhoneNumber = phoneNumber;
            user.UserName = egn;
            user.BirthDate = birthDate;
            user.Gender = gender;

            var updateResult = await _userManager.UpdateAsync(user);
            
            return EditResult(updateResult);
        }

        public async Task<OperationResult> EditDoctor(string id, string firstName, string lastName, string egn, string phoneNumber, DateTime birthDate, string gender)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            if (user == null) return UserNotFoundResult();
            
            user.FirstName = firstName;
            user.LastName = lastName;
            user.EGN = egn;
            user.PhoneNumber = phoneNumber;
            user.BirthDate = birthDate;
            user.Gender = gender;

            var updateResult = await _userManager.UpdateAsync(user);

            return EditResult(updateResult);
        }

        public async Task<string> ForgotPassword(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return null;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<OperationResult> ResetPassword(string token, string username, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(username);
            var pswResetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return PasswordResetResult(pswResetResult);
        }

        public async Task<OperationResult> ChangePassword(string accountId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(accountId);
            
            if (user == null) return UserNotFoundResult();
            
            var res = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            
            if (res == PasswordVerificationResult.Failed) return PasswordIsIncorrectResult();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var pswResetResult = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return PasswordResetResult(pswResetResult);
        }

        public async Task<string> GetUserRole(string accountId)
        {
            var user = await _userManager.FindByIdAsync(accountId);
            
            if (user == null) return null;
            
            var roles = await _userManager.GetRolesAsync(user);

            return roles.First();
        }

        public string GetUserId(ClaimsPrincipal claimsPrincipal) 
            => _userManager.GetUserId(claimsPrincipal);

        #region PrivateMethods

        private OperationResult RegistrationResult(IdentityResult registerResult)
        {
            var registrationResult = new OperationResult();

            registrationResult.Succeeded = false;
            registrationResult.Errors = new List<string>();
            foreach (var err in registerResult.Errors)
                registrationResult.Errors.Add(err.Description);

            return registrationResult;
        }

        private OperationResult EditResult(IdentityResult editResult)
        {
            var result = new OperationResult();

            if (editResult.Succeeded)
                return result;

            result.Succeeded = false;
            result.Errors = new List<string>();

            foreach (var err in editResult.Errors)
                result.Errors.Add(err.Description);

            return result;
        }

        private OperationResult UserNotFoundResult()
        {
            var operationResult = new OperationResult();
            operationResult.Succeeded = false;
            operationResult.Errors = new List<string>();
            operationResult.Errors.Add("Потребителят не съществува.");
            return operationResult;
        }

        private OperationResult PasswordIsIncorrectResult()
        {
            var operationResult = new OperationResult();
            operationResult.Errors = new List<string>();
            operationResult.Succeeded = false;
            operationResult.Errors.Add("Грешна парола.");
            return operationResult;
        }

        private OperationResult PasswordResetResult(IdentityResult pswResetResult)
        {
            var result = new OperationResult();

            if (pswResetResult.Succeeded)
                return result;

            result.Succeeded = false;
            result.Errors = new List<string>();

            foreach (var err in pswResetResult.Errors)
                result.Errors.Add(err.Description);

            return result;
        }
        
        #endregion

    }
}
