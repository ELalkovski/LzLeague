namespace LzLeague.App.Helpers
{
    using LzLeague.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class RoleSeeder
    {
        private static readonly IdentityRole[] roles =
        {
           new IdentityRole("Admin"),
           new IdentityRole("User"), 
        };

        public static async void SeedPrimaryRoles(this IApplicationBuilder app)
        {
            var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceFactory.CreateScope();

            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                    {
                        var result = await roleManager.CreateAsync(role);
                    }
                }
            }
        }
    }
}
