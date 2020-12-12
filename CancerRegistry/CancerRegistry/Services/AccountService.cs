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

        public async Task<bool> LoginPatient(string egn, string password)
        {
            var user = await _userManager.FindByNameAsync(egn);
            if (user == null)
                return false;

            await _signInManager.SignOutAsync();

            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);

            return signInResult.Succeeded;
        }

        public async Task LogoutUser()
         => await _signInManager.SignOutAsync();

        public async Task<bool> RegisterPatient(
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            string password)
        {
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

            if (!registerResult.Succeeded || !roleResult.Succeeded)
                return false;

            return true;
        }
    }
}
