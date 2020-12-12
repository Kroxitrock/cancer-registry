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
        public async Task<IActionResult> LoginPatient(PatientLoginModel login)
        {
            if (!ModelState.IsValid)
                return View("PatientSignInUp");

            var loginResult = await _accountService.LoginPatient(login.EGN, login.Password);

            if (loginResult)
                return RedirectToAction(); //Must redirect to patient's dashboard

            ModelState.AddModelError("", "Login failed: EGN or password invalid.");
            return View("PatientSignInUp");
        }

        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutUser();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient(PatientRegisterModel patient)
        {
            if (!ModelState.IsValid)
                return View(patient);

            var result = await _accountService.RegisterPatient(
                patient.FirstName,
                patient.LastName,
                patient.EGN,
                patient.PhoneNumber,
                patient.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Ops, Something went wrong. Please try again!");
                PatientAccountWrapperModel model = new PatientAccountWrapperModel();
                model.RegisterModel = patient;
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

    }
}
