using Core.Entities;
using Core.Entities.Enum;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<Role> roleManager)
        {
            await roleManager.CreateAsync(new Role { Name = Roles.Admin.ToString() });
            await roleManager.CreateAsync(new Role { Name = Roles.Employee.ToString() });
            await roleManager.CreateAsync(new Role { Name = Roles.Guest.ToString() });
        }
    }
}
