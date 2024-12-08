using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1;

public static class ModelBuilderExtensions
{
    public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext dbContext)
    {

        // Seed organizations
        if (!await dbContext.Organizations.AnyAsync())
        {
            dbContext.Organizations.AddRange(
                new Organization { Id = 1, Name = "Organization One" },
                new Organization { Id = 2, Name = "Organization Two" }
            );
            await dbContext.SaveChangesAsync();
        }

        // Seed roles
        var roles = new[] { "Admin", "SuperAdmin", "User" };
        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Seed users with roles
        var users = new List<(User User, string Role)>
        {
            (new User 
            { 
                UserName = "admin", 
                Email = "admin@example.com", 
                OrganizationId = 1 
            }, "Admin"),
            (new User 
            { 
                UserName = "superadmin", 
                Email = "superadmin@example.com", 
                OrganizationId = 2 
            }, "SuperAdmin"),
            (new User 
            { 
                UserName = "user", 
                Email = "user@example.com", 
                OrganizationId = 1 
            }, "User")
        };

        foreach (var (user, role) in users)
        {
            var userFound = await userManager.FindByNameAsync(user.UserName);
            if (userFound == null)
            {
                // Create the user with a default password
                var result = await userManager.CreateAsync(user, "DefaultPassword1!");
                if (result.Succeeded)
                {
                    // Assign role to the user
                    await userManager.AddToRoleAsync(user, role);
                }
                else
                {
                    // Log errors or handle failures
                    Console.WriteLine($"Failed to create user {user.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}




