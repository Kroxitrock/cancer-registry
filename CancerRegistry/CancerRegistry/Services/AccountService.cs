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

        public List<string> LoginErrors { get; set; }

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

        public async Task Logoutuser()
         => await _signInManager.SignOutAsync();
    }
}
