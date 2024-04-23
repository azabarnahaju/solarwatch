using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication;

public class AuthenticationSeeder
{
    private RoleManager<IdentityRole> roleManager;
    private UserManager<IdentityUser> userManager;
    private IConfiguration config;

    public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, IConfiguration config)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.config = config;
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
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
        
    }

    private async Task CreateAdminIfNotExists()
    {
        var adminInDb = await userManager.FindByEmailAsync(config["AdminInfo:AdminEmail"]);
        if (adminInDb is null)
        {
            Console.WriteLine("admin does not exist");
            var admin = new IdentityUser { UserName = "admin", Email = config["AdminInfo:AdminEmail"] };
            var adminCreated = await userManager.CreateAsync(admin, config["AdminInfo:AdminPassword"]);

            if (adminCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}