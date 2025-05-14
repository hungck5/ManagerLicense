using Microsoft.AspNetCore.Identity;

namespace AuthServer.seed
{
    public static class SeedUser
    {
        public static async Task SeedAdminUser(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync("admin");
            if (user == null)
            {
                var newUser = new IdentityUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(newUser, "Admin@123");
                if (result.Succeeded)
                {
                    Console.WriteLine("User created.");
                }
                else
                {
                    Console.WriteLine("Error: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }
    }

}