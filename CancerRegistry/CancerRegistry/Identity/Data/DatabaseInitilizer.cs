﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var roleExists = await roleManager.RoleExistsAsync("Administrator");
            if (roleExists)
            {
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var config = serviceProvider.GetRequiredService<IConfiguration>();
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

        }
    }
}