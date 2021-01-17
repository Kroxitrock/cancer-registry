using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Identity.Data;
using CancerRegistry.Models.Diagnoses;
using CancerRegistry.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CancerRegistry
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AccountDbContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("AccountDbConnection")));

            services.AddDbContext<DiagnoseContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("AccountDbConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountDbContext>()
                .AddErrorDescriber<BulgarianLanguageIdentityErrorDescriber>()
                .AddDefaultTokenProviders();

            services.AddTransient<AccountService>();
            services.AddTransient<AdministratorService>();
            services.AddTransient<DoctorService>();
            services.AddTransient<PatientService>();
            services.AddTransient<DiagnoseService>();
            services.AddTransient<HealthCheckService>();

            services.AddControllersWithViews();

            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.ConfigureApplicationCookie(options =>
            {
                options.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = context =>
                    {
                        var requestedPath = context.Request.Path;
                        if (requestedPath.Value == "/Admin" || requestedPath.Value == "/admin")
                        {
                            context.Response.Redirect("/Admin/Login");
                        }
                        else if (requestedPath.Value == "/DoctorDashboard" || requestedPath.Value == "/doctordashboard")
                        {
                            context.Response.Redirect("/Account/DoctorSignIn");
                        }
                        else if (requestedPath.Value == "/PatientDashboard" || requestedPath.Value == "/patientdashboard")
                        {
                            context.Response.Redirect("/Account/LoginPatient");
                        }
                        
                        return Task.CompletedTask;
                    }
                };
                
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });
            
            services.AddAuthorization(options => 
            {
                options.AddPolicy("RequireAdministratorRole",
                    policy => policy.RequireRole("Administrator"));
                options.AddPolicy("RequireDoctorRole",
                    policy => policy.RequireRole("Doctor"));
                options.AddPolicy("RequirePatientRole",
                    policy => policy.RequireRole("Patient"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
