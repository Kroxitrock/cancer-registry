using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Models.Accounts.Doctor;
using CancerRegistry.Models.Accounts.Patient;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Models.Diagnoses.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CancerRegistry.Identity.Data
{
    public class DatabaseInitilizer
    {
        //Creates three roles:"Administrator","Doctor", "Patient" into the database if they dont exists
        //Also it creates an account for the user with "Administrator" role if it does not exists
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AccountDbContext>();
            context.Database.EnsureCreated();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            IdentityResult roleResult;

            string[] roleNames = { "Administrator", "Doctor", "Patient" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();


            var roleExists = await roleManager.RoleExistsAsync("Administrator");
            if (roleExists)
            {
                var admin = await userManager.FindByEmailAsync(config["AdminCredentials:Email"]);

                if (admin == null)
                {
                    admin = new ApplicationUser()
                    {
                        UserName = config["AdminCredentials:Email"],
                        Email = config["AdminCredentials:Email"]
                    };
                    IdentityResult result = await userManager.CreateAsync(admin, config["AdminCredentials:Password"]);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(admin, "Administrator");
                }
            }

            roleExists = await roleManager.RoleExistsAsync("Doctor");
            if (roleExists)
            {
                var doctor = await userManager.FindByEmailAsync(config["DoctorCredentials:Email"]);

                if (doctor == null)
                {
                    doctor = new ApplicationUser()
                    {
                        UserName = config["DoctorCredentials:Email"],
                        Email = config["DoctorCredentials:Email"],
                        FirstName = config["DoctorCredentials:FirstName"],
                        LastName = config["DoctorCredentials:LastName"]
                    };

                    IdentityResult result = await userManager.CreateAsync(doctor, config["DoctorCredentials:Password"]);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(doctor, "Doctor");
                        var diagnoseContext = serviceProvider.GetRequiredService<DiagnoseContext>();
                         doctor = await userManager.FindByEmailAsync(config["DoctorCredentials:Email"]);

                         await diagnoseContext.Doctors.AddAsync(new Doctor()
                         {
                             UserId = doctor.Id,
                             EIK = "123456786",
                             DiplomaNum = "D12345678"
                         });
                         await diagnoseContext.SaveChangesAsync();
                    }
                }

            }

            roleExists = await roleManager.RoleExistsAsync("Patient");
            if (roleExists)
            {
                var patient = await userManager.FindByEmailAsync(config["PatientCredentials:Email"]);

                if (patient == null)
                {
                    patient = new ApplicationUser()
                    {
                        UserName = config["PatientCredentials:Email"],
                        Email = config["PatientCredentials:Email"],
                        FirstName = config["PatientCredentials:FirstName"],
                        LastName = config["PatientCredentials:LastName"]
                    };

                    IdentityResult result = await userManager.CreateAsync(patient, config["PatientCredentials:Password"]);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(patient, "Patient");
                        var diagnoseContext = serviceProvider.GetRequiredService<DiagnoseContext>();
                        patient = await userManager.FindByEmailAsync(config["PatientCredentials:Email"]);

                        await diagnoseContext.Patients.AddAsync(new Patient()
                        {
                            UserId = patient.Id,
                            PhoneNumber = 359889261959
                        });
                        await diagnoseContext.SaveChangesAsync();

                        var patientEntity = diagnoseContext.Patients.First();
                        var docotrEntity = diagnoseContext.Doctors.First();

                        await diagnoseContext.HealthChecks.AddAsync(new HealthCheck()
                        {
                            Diagnose = new Diagnose()
                            {
                                Patient = patientEntity,
                                Doctor = docotrEntity,
                                DistantMetastasis = DistantMetastasisState.M0,
                                PrimaryTumor = PrimaryTumorState.T1,
                                RegionalLymphNodes = RegionalLymphNodesState.N0,
                                Stage = 1
                            },
                            Timestamp = DateTime.Now
                        });
                        await diagnoseContext.SaveChangesAsync();

                        var diagnose = diagnoseContext.Diagnoses.First();

                        patientEntity.ActiveDiagnoseId = diagnose.Id;
                        await diagnoseContext.SaveChangesAsync();

                    }
                }

            }

        }
    }
}
