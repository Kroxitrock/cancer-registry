using System;
using System.Threading.Tasks;
using CancerRegistry.Identity;
using CancerRegistry.Identity.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CancerRegistryTests
{
    class ServiceCollectionFactory
    {
        public AccountDbContext DatabaseContext { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public ServiceCollectionFactory()
        {
            var services = new ServiceCollection();

            services
                .AddSingleton<IConfiguration>(new ConfigurationBuilder().Build())
                .AddDbContext<AccountDbContext>(options =>
                    options.UseSqlServer(
                        $"Server=(localdb)\\MSSQLLocalDB;Database=AccountDatabase-{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=true"))
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AccountDbContext>()
                .AddDefaultTokenProviders();

            services.AddLogging();

            services.Configure<IdentityOptions>(config =>
            {
                config.Password.RequiredLength = 4;
                config.Password.RequireDigit = false;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            });

            var provider = services.BuildServiceProvider();

            DatabaseContext = provider.GetRequiredService<AccountDbContext>();
            UserManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            SignInManager = provider.GetRequiredService<SignInManager<ApplicationUser>>();
            SignInManager.Context = new DefaultHttpContext { RequestServices = provider };
            
            DatabaseContext.Database.EnsureCreated();
            DatabaseContext.Database.OpenConnection();
        }

        public static async Task<ServiceCollectionFactory> CreateAsync()
        {
            var services = new ServiceCollectionFactory();

            await CreateRolesAsync(services);
            
            var dummyUser1 = new ApplicationUser() { Id = "1", UserName = "1234"};
            var dummyUser2 = new ApplicationUser() { Id = "2", UserName = "5678" };
            var dummyAdmin = new ApplicationUser() { Id = "3", UserName = "Admin", Email = "admin@admin.com"};
            
            await services.UserManager.CreateAsync(dummyUser1, "qwer");
            await services.UserManager.CreateAsync(dummyUser2, "asdf");
            await services.UserManager.CreateAsync(dummyAdmin, "admin");
            
            await services.UserManager.AddToRoleAsync(dummyUser1, "Patient");
            await services.UserManager.AddToRoleAsync(dummyUser2, "Doctor");
            await services.UserManager.AddToRoleAsync(dummyAdmin, "Administrator");
            
            await services.DatabaseContext.SaveChangesAsync();

            return services;
        }

        private static async Task CreateRolesAsync(ServiceCollectionFactory services)
        {
            string[] roleNames = { "Administrator", "Doctor", "Patient" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await services.RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                    roleResult = await services.RoleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public static void Destroy(ServiceCollectionFactory services)
        {
            services.DatabaseContext.Database.EnsureDeleted();
            services.DatabaseContext.Dispose();
        }
    }
}
