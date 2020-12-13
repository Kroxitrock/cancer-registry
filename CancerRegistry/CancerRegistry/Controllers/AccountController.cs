using CancerRegistry.Identity;
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
                return RedirectToAction("UserDashboard","Home"); 

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

        [HttpGet]
        public async Task<IActionResult> Patient(string id)
        {
            var patient = await _accountService.GetPatient(id);
            return View("PatientProfile", patient);
        }

        public async Task<IActionResult> Edit(string patientId)
        {
            var patient = await _accountService.GetPatient(patientId);
            return View("EditProfilePatient", patient);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string firstName, string lastName, string egn, string phoneNumber)
        {
            var result = await _accountService.Edit(id, firstName, lastName, egn, phoneNumber);

            if (result)
                return RedirectToAction("Patient", "Account", new { id = id });

            ModelState.AddModelError("", "There was an error. Please try again!");
            return RedirectToAction("Edit", id);
        }
    }
}
