using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CancerRegistry.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PatientSignInUp() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginPatient(PatientAccountWrapperModel model)
        {
            if (!ModelState.IsValid)
                return View("PatientSignInUp");

            var loginResult = await _accountService.LoginUser(model.LoginModel.EGN, model.LoginModel.Password);

            if (loginResult)
                return View("/Views/Dashboard/Patient/PatientDashboardIndex.cshtml"); //Must redirect to patient's dashboard

            ModelState.AddModelError("", "Login failed: EGN or password invalid.");
            return View("PatientSignInUp");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult DoctorSignIn() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginDoctor(DoctorLoginModel doctor)
        {
            if (!ModelState.IsValid)
                return View("DoctorSignIn");

            var loginResult = await _accountService.LoginUser(doctor.UID, doctor.Password);

            if (loginResult)
                return RedirectToAction(); //Must redirect to doctor's dashboard

            ModelState.AddModelError("", "Login failed: UID or password invalid.");
            return View("DoctorSignIn");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUser();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient(PatientAccountWrapperModel model)
        {
            if (!ModelState.IsValid)
                return View("PatientSignInUp", model);

            var result = await _accountService.RegisterPatient(
                model.RegisterModel.FirstName,
                model.RegisterModel.LastName,
                model.RegisterModel.EGN,
                model.RegisterModel.PhoneNumber,
                model.RegisterModel.Password);

            if (!result)
            {
                foreach (var err in _accountService.RegisterErrors)
                    ModelState.AddModelError("", err);
                return View("PatientSignInUp", model);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
