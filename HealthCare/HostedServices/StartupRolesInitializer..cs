using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using HealthCare.Models;
using Microsoft.AspNetCore.Identity;

public class StartupRolesInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public StartupRolesInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var roleService = scope.ServiceProvider.GetRequiredService<RoleService>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();


            await roleService.CreateRoleIfNotExists("Admin");
            await roleService.CreateRoleIfNotExists("User");

            var adminEmail = "Sowaity@example.com";
            var adminPassword = "Sowaity123!";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await roleService.AssignRoleToUser(adminUser, "Admin");
                }
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
