using CSharpFunctionalExtensions;
using JourneyApp.Core.Models.User;
using JourneyApp.Core.ValueObjects.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JourneyApp.Infrastructure.Database.Helpers;

public static class DbHelpers
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JourneyAppDbContext>();
        dbContext.Database.Migrate();
    }

    public static async Task SeedAdminRoleAndUser(this IApplicationBuilder app)
    {
        await app.SeedAdminRole();
        await app.SeedAdminUser();
    }

    private static async Task SeedAdminRole(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        if (!await roleManager.RoleExistsAsync(UserRole.Admin.ToString()))
            await roleManager.CreateAsync(new IdentityRole<Guid>(UserRole.Admin.ToString()));
    }
    
    private static async Task SeedAdminUser(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var existingUser = await userManager.FindByEmailAsync("admin@admin.ru");
        if (existingUser is null)
        {
            var (_, _, user, _) = User.Create("admin@admin.ru", "admin", "admin");
            var result = await userManager.CreateAsync(user, "Admin12345!");
            if (!result.Succeeded)
                throw new Exception(string.Join(";", result.Errors.Select(e => e.Description)));
            var addRoleResult = await userManager.AddToRoleAsync(user, UserRole.Admin.ToString());
            if (!addRoleResult.Succeeded)
                throw new Exception(string.Join(";", addRoleResult.Errors));
        }
    }
}