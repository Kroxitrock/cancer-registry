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
                return RedirectToAction("Home","PatientDashboard"); 

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
                return RedirectToAction("", "DoctorDashboard"); //Must redirect to doctor's dashboard

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

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError("", err);
                return View("PatientSignInUp", model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> PatientProfile(string id)
        {
            var patient = await _accountService.GetPatient(id);
            return View("PatientProfile", patient);
        }
        
        [HttpGet]
        public async Task<IActionResult> DoctorProfile(string id)
        {
            var doctor = await _accountService.GetDoctor(id);
            return View("DoctorProfile", doctor);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientProfile(string patientId)
        {
            var patient = await _accountService.GetPatient(patientId);
            var model = new PatientEditProfileModel()
                { 
                    Id = patient.Id,
                    FirstName = patient.FirstName,
                    LastName = patient.LastName,
                    EGN = patient.EGN, 
                    PhoneNumber = patient.PhoneNumber, 
                    Gender = patient.Gender,
                    BirthDate = patient.BirthDate,
                    Genders = new string[] {"Mъж", "Жена"}
                };
            return View("EditProfilePatient", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(PatientEditProfileModel model)
        {
            var result = await _accountService.Edit(model.Id, model.FirstName, model.LastName, model.EGN, model.PhoneNumber, model.BirthDate, model.Gender);

            if (result)
                return RedirectToAction("PatientProfile", "Account", new { id = model.Id });

            ModelState.AddModelError("", "There was an error. Please try again!");
            return RedirectToAction("EditPatientProfile", model.Id);
        }
    }
}
