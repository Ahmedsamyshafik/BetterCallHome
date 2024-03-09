using Domin.Constant;
using Domin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Domin.Seeding
{
    public static class DataSeeder
    {
        public static async void Seed(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Create an  roles if it doesn't exist
            var rolesList = new List<string>
            {
                  Constants.AdminRole, Constants.UserRole, Constants.OwnerRole
            };
            foreach (var role in rolesList)
            {
                var Currenr_Role = roleManager.FindByNameAsync(role).Result; // await vs result
                if (Currenr_Role == null)
                {
                    Currenr_Role = new IdentityRole(role);
                    var result = roleManager.CreateAsync(Currenr_Role).Result;
                    //check result success?
                }
            }


            // Create an admin user if it doesn't exist
            var adminUser = userManager.FindByNameAsync("Admin").Result;
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    //  Id = Guid.NewGuid().ToString(),
                    UserName = "Admin",
                    Email = "Admin@gmail.com",
                    PhoneNumber = "01095385375",
                    //Age = 22,
                    College = "CS",
                    University = "DU",
                    Gender = "M",
                    EmailConfirmed = true,
                    Address = "Domiat"
                };

                var result = userManager.CreateAsync(adminUser, "Alahly1907#").Result;

                // Add the admin user to the admin role
                if (result.Succeeded)
                {
                    userManager.AddToRolesAsync(adminUser, rolesList).GetAwaiter().GetResult();
                }


            }
        }
    }
}
