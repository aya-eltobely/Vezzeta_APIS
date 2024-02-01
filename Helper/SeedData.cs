using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VezetaApi.Models;

namespace VezetaApi.Helper
{
    public static class SeedData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();


            //create Roles
            if (!roleManager.RoleExistsAsync(WebSiteRoles.SiteAdmin).GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole( WebSiteRoles.SiteAdmin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SitePatient)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(WebSiteRoles.SiteDoctor)).GetAwaiter().GetResult();
            }

            //Create Admin
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "Aya",
                Email = "admin@gmail.com"
            }, "Awxiokoi6$").GetAwaiter().GetResult();

            //check admin exist
            var AppAdmin = userManager.FindByEmailAsync("admin@gmail.com").GetAwaiter().GetResult();

            //asign role to admin
            if (AppAdmin != null)
            {
                userManager.AddToRoleAsync(AppAdmin, WebSiteRoles.SiteAdmin).GetAwaiter().GetResult();
            }
            //userManager.AddClaimAsync(AppAdmin, new Claim(ClaimTypes.Role,WebSiteRoles.SiteAdmin)).GetAwaiter().GetResult();

            ////////////// 

            if (!dbContext.Specializations.Any())
            {
                // Add initial data
                dbContext.Specializations.AddRange(
                    new Specialization { Name = "Anesthesiologist" },
                    new Specialization { Name = "Cardiologist" },
                    new Specialization { Name = "neurologist" },
                    new Specialization { Name = "Dentist" },
                    new Specialization { Name = "Emergency Doctors" }
                    // Add more as needed
                );

                // Save changes to the database
                dbContext.SaveChanges();
            }

            dbContext.SaveChanges();




        }
    }
}
