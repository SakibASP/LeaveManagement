using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeaveManagement.Infrustructure.UserModel;
using LeaveManagement.ViewModels;

namespace LeaveManagement.Web.Controllers
{
    [Authorize]
    public class MaintainUsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;

        public async Task<IActionResult> Index(string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
                ViewBag.searchString = searchString;

            if (currentFilter != null)
                ViewBag.Filter = currentFilter;

            ViewBag.CurrentFilter = new SelectList(_roleManager.Roles, "Id", "Name");

            IList<ApplicationUser> users = await _userManager.Users.ToListAsync();
            if (!string.IsNullOrEmpty(currentFilter))
            {
                // Get the role by ID
                var role = await _roleManager.FindByIdAsync(currentFilter);
                users = await _userManager.GetUsersInRoleAsync(role.Name);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(x => x.UserName.ToLower().Contains(searchString.ToLower()) || x.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }

            IList<UserRolesViewModel> userRolesViewModel = [];
            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = await GetUserRoles(user)
                };
                userRolesViewModel.Add(thisViewModel);
            }

            return View(userRolesViewModel);
        }
        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }
        public async Task<IActionResult> Manage(string userId)
        {
            ViewData["userId"] = userId;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
            ViewData["UserName"] = user.UserName;
            var roles = await _roleManager.Roles.ToListAsync();
            var model = new List<ManageUserRolesViewModel>();
            //adding all roles to superadmin but not to others

            foreach (var role in roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name!))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                model.Add(userRolesViewModel);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Manage(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View();
            }
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove user existing roles");
                return View(model);
            }
            result = await _userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName)!);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected roles to user");
                return View(model);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return View();
                }

                //removing the user
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user");
                    return View();
                }
                else
                {
                    TempData["Success"] = "Removed Successfully!";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed! Something went wrong!";
            }
            return RedirectToAction("Index");
        }

    }
}
