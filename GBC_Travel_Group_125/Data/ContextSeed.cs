using GBC_Travel_Group_125.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace GBC_Travel_Group_125.Data
{
    public class ContextSeed
    {
        // Predefined role names
        private static readonly string[] roles = new string[] {
            "SuperAdmin",
            "Admin",
            "Moderator",
            "Basic"
        };

        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles)
            {
                await CreateRole(roleManager, role);
            }
        }

        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    // Log or handle the error
                    throw new Exception($"Failed to create role: {roleName}, Error: {roleResult.Errors.FirstOrDefault()}");
                }
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var superAdminEmail = "superAdmin@a.ca";
            var superAdminUserName = superAdminEmail; // Use email as username for consistency

            // Check if the super user already exists based on email, which should be normalized
            var user = await userManager.FindByEmailAsync(superAdminEmail);
            if (user == null)
            {
                var superUser = new ApplicationUser
                {
                    UserName = superAdminUserName,
                    Email = superAdminEmail,
                    FirstName = "Super",
                    LastName = "Admin",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                IdentityResult userResult = await userManager.CreateAsync(superUser, "Bilmem555.");
                if (!userResult.Succeeded)
                {
                    throw new Exception($"Failed to create super user: {userResult.Errors.FirstOrDefault()}");
                }

                await AssignRoles(userManager, superUser.Email, roles);
            }
        }

        private static async Task AssignRoles(UserManager<ApplicationUser> userManager, string email, string[] roles)
        {
            var user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to assign roles: {result.Errors.FirstOrDefault()}");
            }
        }
    }
}
