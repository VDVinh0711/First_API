using API_EF.Data;
using API_EF.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StackExchange.Redis;
using System.Security.Claims;

namespace API_EF.Service
{
    public static class SeedingData
    {

        //Create scope for database
        //Add more Funtion if you want
        
        public static async Task<IApplicationBuilder> SeedDataAsync(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                var datacontext = scope.ServiceProvider.GetRequiredService<DataContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                //It can remove database , creat and add data
                // it usually use to create new or refesh database when you call it
                await datacontext.Database.EnsureDeletedAsync();
                if(await datacontext.Database.EnsureCreatedAsync()) 
                {
                    //Dosomething when creat data
                    var adminrole = new IdentityRole(TypeSafe.Roles.Admin);
                    var employeerole = new IdentityRole(TypeSafe.Roles.Employee);

                    var adminUser = new IdentityUser() { UserName = "admin", Email = "lagger@gmail.com" };

                    await userManager.CreateAsync(adminUser, "P@ssw0rld");



                    await userManager.AddClaimAsync(adminUser, new Claim("AdmiClaim User", "value"));

                    await userManager.AddToRoleAsync(adminUser, TypeSafe.Roles.Admin);
                    await roleManager.AddClaimAsync(adminrole, new Claim("Admin Claim Role", "value1"));




                }
            }
            return app;
        }
    }
}
