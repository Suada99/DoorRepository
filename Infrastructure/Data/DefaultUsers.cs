using Core.Entities;
using Core.Entities.Enum;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data
{
    public static class DefaultUsers
    {
        public static async Task SeedSuperAdminAsync(UserManager<User> userManager)
        {
            var defaultUser = new User
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                Tag = new Tag
                {
                    Id = Guid.NewGuid(),
                    Status = TagStatus.Active,
                    StartDate = DateTime.Now,
                    ExpireDate = DateTime.Now.AddYears(3)
                }
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Suada12345!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }

}
