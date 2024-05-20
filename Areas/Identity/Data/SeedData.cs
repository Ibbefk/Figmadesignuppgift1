using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Figmadesign.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                context.Database.Migrate();

                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                if (!context.Users.Any())
                {
                    var user = new IdentityUser { UserName = "testuser@example.com", Email = "testuser@example.com" };
                    await userManager.CreateAsync(user, "Password123!");
                }
            }
        }
    }
}
