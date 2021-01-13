using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CancerRegistry.Services;
using Microsoft.AspNetCore.Identity;
using NUnit.Framework;

namespace CancerRegistryTests
{
    [TestFixture]
    class AccountServiceIntegrationTests
    {
        private ServiceCollectionFactory _services;

        [Test]
        [TestCase("1234", "qwer")]
        public async Task LoginPatient_WhenPatientSuccessfullyLogsIn_ReturnsTrue(string username, string password)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var result = await accountService.LoginUser(username, password);
            var user = await _services.UserManager.FindByNameAsync(username);
            var isPatient = await _services.UserManager.IsInRoleAsync(user, "Patient");

            Assert.True(isPatient);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("123", "Password98!")]
        [TestCase("5647", "Pass36!")]
        public async Task LoginPatient_WhenPatientFailsToLogIn_ReturnsFalse(string username, string password)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var result = await accountService.LoginUser(username, password);
            Assert.IsFalse(result);
        }

        [Test]
        [TestCase("Qnko", "Qnkov", "9505131234", "0965342312", "Password123!")]
        [TestCase("Ivaylo", "Petrov", "9302236456", "0965235132", "Password456!")]
        public async Task RegisterPatient_WhenPatientSuccessfullyRegisters_NoErrors(
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            string password)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var result = await accountService.RegisterPatient(firstName, lastName, egn, phoneNumber, password);
            var user = await _services.UserManager.FindByNameAsync(egn);

            Assert.IsNull(result.Errors);
            Assert.IsTrue(result.Succeeded);
            Assert.IsTrue(user.UserName == egn);
        }

        [Test]
        [TestCase("Qnko", "Qnkov", "1234", "0965342312", "Password123!")]
        [TestCase("Ivaylo", "Petrov", "5678", "0965235132", "Password456!")]
        public async Task RegisterPatient_WhenPatientFailsToRegister_WithErrors(
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            string password)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var result = await accountService.RegisterPatient(firstName, lastName, egn, phoneNumber, password);

            var users = _services.UserManager.Users.ToList();

            Assert.IsTrue(users.Count == 3);
            Assert.IsNotNull(result.Errors);
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        [TestCase("1", "Kamen", "Ivanov", "1234", "0878992254", "12/05/1999", "Мъж")]
        public async Task EditPatientProfile_SuccessfullEdit_NoErrors(
            string id,
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            DateTime birthDate,
            string gender)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var result = await accountService.EditPatient(id, firstName, lastName, egn, phoneNumber, birthDate, gender);

            Assert.IsNull(result.Errors);
            Assert.IsTrue(result.Succeeded);
        }

        [Test]
        [TestCase("1", "Kamen", "Ivanov", "5678", "0878992254", "12/05/1999", "Мъж")]
        public async Task EditPatientProfile_FailedEdit_WithErrors(
            string id,
            string firstName,
            string lastName,
            string egn,
            string phoneNumber,
            DateTime birthDate,
            string gender)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var result = await accountService.EditPatient(id, firstName, lastName, egn, phoneNumber, birthDate, gender);

            Assert.IsNotNull(result.Errors);
            Assert.IsFalse(result.Succeeded);
        }

        [Test]
        [TestCase("1234")]
        public async Task ForgotPassword_WhenUserIsFound_ReturnsToken(string username)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var token = await accountService.ForgotPassword(username);

            Assert.IsNotNull(token);
        }

        [Test]
        [TestCase("123467")]
        public async Task ForgotPassword_WhenUserIsNotFound_ReturnsNull(string username)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var token = await accountService.ForgotPassword(username);

            Assert.IsNull(token);
        }

        [Test]
        [TestCase("1234", "Password123!")]
        public async Task ResetPassword_SuccessfullPasswordReset_NoErrors(string username, string newPassword)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var token = await accountService.ForgotPassword(username);
            var result = await accountService.ResetPassword(token, username, newPassword);

            var user = await _services.UserManager.FindByNameAsync(username);
            var isPasswordChanged = _services.UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);

            Assert.IsNull(result.Errors);
            Assert.IsTrue(isPasswordChanged == PasswordVerificationResult.Success && result.Succeeded);
        }

        [Test]
        [TestCase("1", "qwer", "Password123!")]
        public async Task ChangePassword_SuccessfulPasswordChange_NoErrors(string accountId, string currentPassword, string newPassword)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var result = await accountService.ChangePassword(accountId, currentPassword, newPassword);
            
            var user = await _services.UserManager.FindByIdAsync(accountId);
            var isPasswordChanged = _services.UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);

            Assert.IsNull(result.Errors);
            Assert.IsTrue(isPasswordChanged == PasswordVerificationResult.Success && result.Succeeded);
        }

        [Test]
        [TestCase("1", "123", "Password123!")]
        public async Task ChangePassword_CurrentPasswordIsWrong_WithErros(string accountId, string currentPassword, string newPassword)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);
            var result = await accountService.ChangePassword(accountId, currentPassword, newPassword);
            
            var user = await _services.UserManager.FindByIdAsync(accountId);
            var isPasswordChanged = _services.UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, newPassword);

            Assert.IsNotNull(result.Errors);
            Assert.IsFalse(isPasswordChanged == PasswordVerificationResult.Success && result.Succeeded);
        }

        [Test]
        [TestCase("1")]
        public async Task GetUserRole_UserIsOfRolePatient(string accountId)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var userRole = await accountService.GetUserRole(accountId);
            
            Assert.IsTrue(userRole == "Patient");
        }

        [Test]
        [TestCase("2")]
        public async Task GetUserRole_UserIsOfRoleDoctor(string accountId)
        {
            var accountService = new AccountService(_services.UserManager, _services.SignInManager);

            var userRole = await accountService.GetUserRole(accountId);

            Assert.IsTrue(userRole == "Doctor");
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
