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

        public List<string> RegisterErrors { get; private set; }

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            RegisterErrors = new List<string>();
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
            {
                RegisterErrors.Clear();

                foreach (var err in registerResult.Errors)
                    RegisterErrors.Add(err.Description);

                foreach (var err in roleResult.Errors)
                    RegisterErrors.Add(err.Description);

                return false;
            }

            return true;
        }

        public async Task<ApplicationUser> GetPatient(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<bool> Edit(string id, string firstName, string lastName, string egn, string phoneNumber)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return false;

            user.FirstName = firstName;
            user.LastName = lastName;
            user.EGN = egn;
            user.PhoneNumber = phoneNumber;
            user.UserName = egn;

            await _userManager.UpdateAsync(user);
            return true;
        }

    }
}
