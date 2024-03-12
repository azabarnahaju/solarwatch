using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private UserManager<IdentityUser> userManager;
    private Dictionary<string, string> adminInfo;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, Dictionary<string, string> adminInfo)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.adminInfo = adminInfo;
    }

    public void AddRoles()
    {
        var tAdmin = CreateRole("Admin");
        tAdmin.Wait();

        var tUser = CreateRole("User");
        tUser.Wait();
    }

    public void AddAdmin()
    {
        var tAdmin = CreateAdminIfNotExists();
        tAdmin.Wait();
    }
    
    private async Task CreateRole(string role)
    {
        await roleManager.CreateAsync(new IdentityRole(role));
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync("admin@admin.com");
        if (adminInDb is null)
        {
            var admin = new IdentityUser { UserName = "admin", Email = adminInfo["adminEmail"] };
            var adminCreated = await userManager.CreateAsync(admin, adminInfo["adminPassword"]);

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}