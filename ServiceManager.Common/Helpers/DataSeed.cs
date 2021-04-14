using Microsoft.AspNetCore.Identity;
using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceManager.Common.Helpers
{
    public class DataSeed
    {
        public static void SeedRoles(RoleManager<SystemRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync
        ("Administrator").Result)
            {
                SystemRole role = new SystemRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }


            if (!roleManager.RoleExistsAsync
        ("Administrator").Result)
            {
                SystemRole role = new SystemRole();
                role.Name = "Administrator";
                IdentityResult roleResult = roleManager.
                CreateAsync(role).Result;
            }
        }

        public static void SeedUsers(UserManager<SystemUser> userManager)
        {
            if (userManager.FindByNameAsync
        ("user1").Result == null)
            {
                SystemUser user = new SystemUser();
                user.UserName = "majid.k";
                user.Email = "majid.k@allshorestaffing.com";
                user.FirstName = "Majid Ali Khan";
                user.LastName = "Quaid";
                //user.Project = "Ferrellgas";

                IdentityResult result = userManager.CreateAsync(user, "National$0").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }


            if (userManager.FindByNameAsync
        ("majid.k").Result == null)
            {
                SystemUser user = new SystemUser();
                user.UserName = "majid.k";
                user.Email = "majid.k@allshorestaffing.com";
                user.FirstName = "Majid Ali Khan";
                user.LastName = "Quaid";
                //user.Project = "Ferrellgas";

                IdentityResult result = userManager.CreateAsync
                (user, "National$0").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user,
                                        "Administrator").Wait();
                }
            }
        }

    }
}
