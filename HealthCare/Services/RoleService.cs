using HealthCare.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

public class RoleService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ILogger<RoleService> _logger;
    private readonly UserManager<AppUser> _userManager;


    public RoleService(RoleManager<IdentityRole<int>> roleManager, ILogger<RoleService> logger, UserManager<AppUser> userManager)
    {
        _roleManager = roleManager;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task CreateRoleIfNotExists(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<int> { Name = roleName });
            if (result.Succeeded)
            {
                _logger.LogInformation($"Role '{roleName}' created successfully.");
            }
            else
            {
                _logger.LogError($"Error creating role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
        else
        {
            _logger.LogInformation($"Role '{roleName}' already exists.");
        }
    }

    public async Task<IdentityResult> AssignRoleToUser(AppUser user, string roleName)
    {
        return await _userManager.AddToRoleAsync(user, roleName);
    }
}
