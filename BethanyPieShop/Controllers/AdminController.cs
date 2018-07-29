using BethanyPieShop.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BethanyPieShop.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public UserManager<IdentityUser> userManager;

        public AdminController(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
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

            var user = new IdentityUser()
            {
                UserName = addUserViewModel.UserName,
                Email = addUserViewModel.Email
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

            return View(user);
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
            IdentityUser user = await userManager.FindByIdAsync(userId);

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
    }
}