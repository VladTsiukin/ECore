using ECore.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace ECore.Domain.Context
{

    public static class ECoreIdentityInitializer
    {
        private const string adminUser = "admin";
        private const string adminPassword = "admin";
        private const string adminEmail = "victestvt@yandex.ru";
        private const string adminRole = "adminvictestvt";

        public static async Task EnsurePopulated(UserManager<AppUser> userManager,
                                                 RoleManager<IdentityRole> roleManager)
        {

            AppUser user = await userManager.FindByEmailAsync("victestvt@yandex.ru");

            if (user == null)
            {
                user = new AppUser();
                user.UserName = adminUser;
                user.Email = adminEmail;
                user.EmailConfirmed = true;

                IdentityResult resRole
                   = await roleManager.CreateAsync(new IdentityRole(adminRole));

                IdentityResult result = await userManager.CreateAsync(user, adminPassword);

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.NormalizedUserName),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, adminRole),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                if (result.Succeeded)
                {
                    // create role
                    await userManager.AddClaimsAsync(user, claims);
                    await userManager.AddToRoleAsync(user, adminRole);
                    Console.WriteLine("EnsurePopulated(UserManager<AppUser> userManager => SUCCESS.");
                }
                else
                {
                    Console.WriteLine("EnsurePopulated(UserManager<AppUser> userManager => FAIL.");
                }
            }
        }
    }
}
