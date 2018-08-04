using BethanyPieShop.ApplicationDbContext;
using BethanyPieShop.Auth;
using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public readonly UserManager<ApplicationUser> userManager;
        public readonly RoleManager<IdentityRole> roleManager;
        public readonly AppDbContext appDbContext;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appDbContext = appDbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserManagement()
        {
            var users = userManager.Users;

            return View(users);
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid) return View(addUserViewModel);

            var user = new ApplicationUser()
            {
                UserName = addUserViewModel.UserName,
                Email = addUserViewModel.Email,
                Birthdate = addUserViewModel.Birthdate,
                City = addUserViewModel.City,
                Country = addUserViewModel.Country
            };

            IdentityResult result = await userManager.CreateAsync(user, addUserViewModel.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("UserManagement", userManager.Users);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addUserViewModel);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
                return RedirectToAction("UserManagement", userManager.Users);

            var claims = await userManager.GetClaimsAsync(user);
            var vm = new EditUserViewModel() { Id = user.Id, Email = user.Email, UserName = user.UserName, UserClaims = claims.Select(c => c.Value).ToList() };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(string id, string UserName, string Email)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.Email = Email;
                user.UserName = UserName;

                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("UserManagement", userManager.Users);

                ModelState.AddModelError("", "User not updated, something went wrong.");

                return View(user);
            }

            return RedirectToAction("UserManagement", userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("UserManagement");
                else
                    ModelState.AddModelError("", "Something went wrong while deleting this user.");
            }
            else
            {
                ModelState.AddModelError("", "This user can't be found");
            }
            return View("UserManagement", userManager.Users);
        }

        //Roles management
        public IActionResult RoleManagement()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        public IActionResult AddNewRole() => View();

        [HttpPost]
        public async Task<IActionResult> AddNewRole(AddRoleViewModel addRoleViewModel)
        {

            if (!ModelState.IsValid) return View(addRoleViewModel);

            var role = new IdentityRole
            {
                Name = addRoleViewModel.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(addRoleViewModel);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RoleManagement", roleManager.Roles);
            }

            var editRoleViewModel = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                    editRoleViewModel.Users.Add(user.UserName);
            }

            return View(editRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await roleManager.FindByIdAsync(editRoleViewModel.Id);

            if (role != null)
            {
                role.Name = editRoleViewModel.RoleName;

                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", roleManager.Roles);

                ModelState.AddModelError("", "Role not updated, something went wrong.");

                return View(editRoleViewModel);
            }

            return RedirectToAction("RoleManagement", roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("RoleManagement", roleManager.Roles);
                ModelState.AddModelError("", "Something went wrong while deleting this role.");
            }
            else
            {
                ModelState.AddModelError("", "This role can't be found.");
            }
            return View("RoleManagement", roleManager.Roles);
        }

        //Users in roles
        public async Task<IActionResult> AddUserToRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id };

            foreach (var user in userManager.Users)
            {
                if (!await userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        public async Task<IActionResult> DeleteUserFromRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
                return RedirectToAction("RoleManagement", roleManager.Roles);

            var addUserToRoleViewModel = new UserRoleViewModel { RoleId = role.Id };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    addUserToRoleViewModel.Users.Add(user);
                }
            }

            return View(addUserToRoleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserFromRole(UserRoleViewModel userRoleViewModel)
        {
            var user = await userManager.FindByIdAsync(userRoleViewModel.UserId);
            var role = await roleManager.FindByIdAsync(userRoleViewModel.RoleId);

            var result = await userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("RoleManagement", roleManager.Roles);
            }

            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(userRoleViewModel);
        }

        //Claims
        public async Task<IActionResult> ManageClaimsForUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return RedirectToAction("UserManagement", userManager.Users);

            var claimsManagementViewModel = new ClaimsManagementViewModel { UserId = user.Id, AllClaimsList = BethanysPieShopClaimTypes.ClaimsList };

            return View(claimsManagementViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageClaimsForUser(ClaimsManagementViewModel claimsManagementViewModel)
        {
            var user = await userManager.FindByIdAsync(claimsManagementViewModel.UserId);

            if (user == null)
                return RedirectToAction("UserManagement", userManager.Users);
            //user.Claims.Add(new IdentityUserClaim<string>
            //{
            //    ClaimType = claimsManagementViewModel.ClaimId,
            //    ClaimValue = claimsManagementViewModel.ClaimId
            //}););
            IdentityUserClaim<string> claim =
                new IdentityUserClaim<string> { ClaimType = claimsManagementViewModel.ClaimId, ClaimValue = claimsManagementViewModel.ClaimId };

            appDbContext.UserClaims.Add(claim);
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return RedirectToAction("UserManagement", userManager.Users);

            ModelState.AddModelError("", "User not updated, something went wrong.");

            return View(claimsManagementViewModel);
        }
    }
}