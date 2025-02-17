using LeaveManagement.Infrustructure.UserModel;
using LeaveManagement.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Web.Data.Roles
{
    public enum Roles
    {
        Admin,
        Employee,
        Manager
    }
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            var Admin = roleManager.FindByNameAsync(Roles.Admin.ToString());
            var Employee = roleManager.FindByNameAsync(Roles.Employee.ToString());
            var Manager = roleManager.FindByNameAsync(Roles.Manager.ToString());
            if (Manager is null) await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
            if (Admin is null) await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            if (Employee is null) await roleManager.CreateAsync(new IdentityRole(Roles.Employee.ToString()));
        }
        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "sakibur.rahman.cse@gmail.com",
                Email = "sakibur.rahman.cse@gmail.com",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Sakib@123");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Employee.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }

            }
        }
    }
}
