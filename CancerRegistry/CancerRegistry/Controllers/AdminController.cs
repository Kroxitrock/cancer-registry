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

        public AdminController(AdministratorService adminService) 
            => _adminService = adminService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View();
        
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
        public IActionResult Home()
        {
            var users = _adminService.GetAllUsers();
            return View(users);
        }

        public IActionResult RegisterDoctor() => View();

        [HttpPost]
        public async Task<IActionResult> CreateDoctor(DoctorRegisterModel doctor)
        {
            if (!ModelState.IsValid)
                return View(doctor);

            var result = await _adminService
                .RegisterDoctor(
                    doctor.FirstName,
                    doctor.LastName,
                    doctor.EGN,
                    doctor.UID);

            if (result.Succeeded) 
                return RedirectToAction("Index");
            
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
            
            return View(doctor);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(string id)
        {
            var result = await _adminService.DeleteUser(id);

            if (result)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "User not found!");
            //should redirect to view
            return RedirectToAction("Index");
        }
    }
}
