using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeretanaBackend.Models;
using TeretanaBackend.ViewModels;

namespace TeretanaBackend.Data
{
    public class AppDbInitializer
    {
        public static async Task SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if(!await roleManager.RoleExistsAsync(UserRoles.Admin)) await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await roleManager.RoleExistsAsync(UserRoles.User)) await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                if (!await roleManager.RoleExistsAsync(UserRoles.Publisher)) await roleManager.CreateAsync(new IdentityRole(UserRoles.Publisher));

                if (!await roleManager.RoleExistsAsync(UserRoles.PersonalTrainer)) await roleManager.CreateAsync(new IdentityRole(UserRoles.PersonalTrainer));
            }
        }
        public static async Task SeedStatusOfOrders(IApplicationBuilder applicationBuilder)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                if (!await dbContext.StatusOrders.AnyAsync())
                {
                    var statuses = new List<StatusOrder> {
                        new StatusOrder(){StatusDescrption=StatusOfOrders.UnpayedOrder,CreatedBy="SYSTEM ADMIN",CreatedAt=DateTime.UtcNow},
                        new StatusOrder(){StatusDescrption=StatusOfOrders.AbortedOrder,CreatedBy="SYSTEM ADMIN",CreatedAt=DateTime.UtcNow},
                        new StatusOrder(){StatusDescrption=StatusOfOrders.PayedOrder,CreatedBy="SYSTEM ADMIN",CreatedAt=DateTime.UtcNow}
                    };
                    await dbContext.StatusOrders.AddRangeAsync(statuses);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
