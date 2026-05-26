using Kintech.Models;
using Microsoft.AspNetCore.Identity;

public static class DbInitializer
{
    public static async Task Seed(UserManager<ApplicationUser> userManager,
                                 RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var adminEmail = "admin@kintech.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail
            };

            await userManager.CreateAsync(newAdmin, "Admin@123");

            await userManager.AddToRoleAsync(newAdmin, "Admin");
        }
    }
}