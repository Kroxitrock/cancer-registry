using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Services;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace CancerRegistryTests
{
    [TestFixture]
    class AdministratorServiceIntegrationTests
    {
        private ServiceCollectionFactory _services;
        
        [Test]
        [TestCase("Admin", "admin")]
        public async Task LoginAdmin_WhenAdminSuccessfullyLogsIn_ReturnsTrue(string username, string password)
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);
            var result = await adminService.LoginAdmin(username, password);
            var user = await _services.UserManager.FindByNameAsync(username);
            var isAdmin = await _services.UserManager.IsInRoleAsync(user, "Administrator");
            
            Assert.True(isAdmin);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("1234", "qwer")]
        [TestCase("5647", "Pass36!")]
        public async Task LoginAdmin_WhenPersonIsNotAdmin_ReturnsFalse(string username, string password)
        {
            var adminService= new AdministratorService(_services.UserManager, _services.SignInManager);
            var result = await adminService.LoginAdmin(username, password);
            Assert.IsFalse(result);
        }
        [Test]
        [TestCase("2")]
        public async Task DeleteUser_NoErrors(string id)
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);
            var result = await adminService.DeleteUser(id);
            
            Assert.IsTrue(_services.UserManager.Users.Count() < 3);
            Assert.IsNull(result.Errors);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        [TestCase("4")]
        public async Task DeleteUser_WithErrors(string id)
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);
            var result = await adminService.DeleteUser(id);

            Assert.IsTrue(_services.UserManager.Users.Count() == 3);
            Assert.IsNotNull(result.Errors);
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        [TestCase("Qnko", "Qnkov", "9505131234", "F12345")]
        [TestCase("Ivaylo", "Petrov", "9302236456", "G6R3223")]
        public async Task RegisterDoctor_SuccessfulRegistration_NoErrors(
            string firstName,
            string lastName,
            string egn,
            string uid)
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);

            var result = await adminService.RegisterDoctor(firstName, lastName, egn, uid);
            var users = _services.UserManager.Users.ToList();

            Assert.IsNull(result.Errors);
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(users.Count > 3);
        }

        [Test]
        [TestCase("Qnko", "Qnkov", "9505131234", "5678")]
        public async Task RegisterDoctor_RegistrationFails_WithErrors(
            string firstName,
            string lastName,
            string egn,
            string uid)
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);

            var result = await adminService.RegisterDoctor(firstName, lastName, egn, uid);
            var users = _services.UserManager.Users.ToList();

            Assert.IsNotNull(result.Errors);
            Assert.IsFalse(result.Succeeded);
            Assert.IsTrue(users.Count == 3);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOnlyDoctorsAndPatients()
        {
            var adminService = new AdministratorService(_services.UserManager, _services.SignInManager);
            var users = await adminService.GetAllUsers();
            var usersRoles = 
                users.Select(async x => await _services.UserManager.GetRolesAsync(x))
                .Select(t=>t.Result.First());
            Assert.That(usersRoles.All(x=> x == "Doctor" || x == "Patient"));
        }
        
        [SetUp]
        public async Task Setup()
        {
            _services = await ServiceCollectionFactory.CreateAsync();
        }

        [TearDown]
        public void TearDown()
        {
            ServiceCollectionFactory.Destroy(_services);
        }
    }
}
