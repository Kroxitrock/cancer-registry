using CancerRegistry.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Models.Accounts.Doctor;
using Microsoft.AspNetCore.Authorization;
using CancerRegistry.Models.Admin;

namespace CancerRegistry.Controllers
{   
    [Authorize(Policy = "RequireAdministratorRole")]
    public class AdminController : Controller
    {
        private readonly AdministratorService _adminService;
        private readonly DoctorService _doctorService;
        private readonly PatientService _patientService;

        public AdminController(AdministratorService adminService, DoctorService doctorService, PatientService patientService)
        {
            _adminService = adminService;
            _doctorService = doctorService;
            _patientService = patientService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AdminLoginModel admin)
        {
            if (!ModelState.IsValid)
                return View(admin);

            var result = await _adminService.LoginAdmin(admin.UserName, admin.Password);

            if (result)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Login failed. Username or password invalid.");
            return View(admin);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await _adminService.Logout();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateDoctor()
        {
            CreateDoctorModel model = new CreateDoctorModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(CreateDoctorModel doctor)
        {
            if (!ModelState.IsValid)
                return View(doctor);

            var result = await _adminService
                .RegisterDoctor(
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.EGN,
                    doctor.UID,
                    doctor.Gender,
                    doctor.Bulstat);

            if (result.Succeeded)
            {
                var d = await _adminService.GetUserByName(doctor.UID);
                await _doctorService.CreateDoctor(d.Id, doctor.UID);
                return RedirectToAction("Index");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
            
            return View(doctor);
        }

        [HttpGet]
        public async Task<IActionResult> AllPatients()
        {
            var patients = await _adminService.GetAllPatients();
            return View("Patients", patients);
        }

        [HttpGet]
        public async Task<IActionResult> AllDoctors()
        {
            var doctors = await _adminService.GetAllDoctors();
            return View("Doctors", doctors);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var isPatient = await _adminService.IsPatient(id);
            var isDoctor = await _adminService.IsDoctor(id);

            var result = await _adminService.DeleteUser(id);

            if (result.Succeeded)
            {
                if (isPatient) await _patientService.DeletePatient(id);
                if (isDoctor) await  _doctorService.DeleteDoctor(id);
                
                return RedirectToAction("Index");
            }

            foreach(var err in result.Errors)
                ModelState.AddModelError("", err);

            return View("Index");
        }
    }
}
