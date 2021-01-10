﻿using CancerRegistry.Identity;
using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Models.Accounts;

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
                return RedirectToAction("Home", "PatientDashboard");

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
                Genders = new string[] { "Mъж", "Жена" }
            };

            return View("EditProfilePatient", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(PatientEditProfileModel model)
        {
            var result = await _accountService.EditPatient(model.Id, model.FirstName, model.LastName, model.EGN, model.PhoneNumber, model.BirthDate, model.Gender);

            if (result)
                return RedirectToAction("PatientProfile", "Account", new { id = model.Id });

            ModelState.AddModelError("", "The EGN is already in use!");
            return RedirectToAction("EditPatientProfile", "Account", model.Id);
        }

        [HttpGet]
        public async Task<IActionResult> EditDoctorProfile(string doctorId)
        {
            var doctor = await _accountService.GetDoctor(doctorId);

            var model = new DoctorEditProfileModel()
            {
                Id = doctor.Id,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                EGN = doctor.EGN,
                PhoneNumber = doctor.PhoneNumber,
                Gender = doctor.Gender,
                BirthDate = doctor.BirthDate,
                Genders = new string[] { "Mъж", "Жена" }
            };

            return View("EditProfileDoctor", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditDoctorProfile(DoctorEditProfileModel model)
        {
            var result = await _accountService.EditDoctor(model.Id, model.FirstName, model.LastName, model.EGN, model.PhoneNumber, model.BirthDate, model.Gender);

            if (result)
                return RedirectToAction("DoctorProfile", "Account", new { id = model.Id });

            ModelState.AddModelError("", "The EGN is already in use!");
            return RedirectToAction("EditDoctorProfile", "Account", new { doctorId = model.Id });
        }



        [HttpGet]
        public IActionResult ChangePassword(string accountId)
        {
            var model = new ChangePassword { AccountId = accountId };
            return View("ChangePassword", model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _accountService.ChangePassword(model.AccountId, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var userRole = await _accountService.GetUserRole(model.AccountId);
                return RedirectToAction(userRole == "Patient" ? "PatientProfile" : "DoctorProfile", new {id = model.AccountId});
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string username)
        {
            if (!ModelState.IsValid)
                return View(username);

            var token = await _accountService.ForgotPassword(username);

            if (token == null)
            {
                ModelState.AddModelError("", "Username couldn't be found.");
                return RedirectToAction("ForgotPassword");
            }


            return RedirectToAction("PasswordReset", new { token, username });
        }

        public IActionResult PasswordReset(string token, string username)
            => View(new PasswordReset() { Token = token, Username = username });

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordReset(PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
                return View(passwordReset);

            var pswResetResult = await _accountService.ResetPassword(
                passwordReset.Token,
                passwordReset.Username,
                passwordReset.Password);

            if (!pswResetResult)
            {
                ModelState.AddModelError("", "We couldn't reset your password. Please, try again!");
                return View();
            }

            return View("PasswordResetSuccess");

        }

        public IActionResult AccessDenied()
            => View();
    }
}
