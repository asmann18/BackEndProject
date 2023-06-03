using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Models;
using Back_End_Project.Utilits;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]

    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<AppUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
           var users =_userManager.Users.ToList();
            List<AllUserViewModel> allUserViewModels = new List<AllUserViewModel>();
            foreach (AppUser appUser in users)
            {
                var role =await _userManager.GetRolesAsync(appUser);
                AllUserViewModel allUserViewModel = new()
                {
                    Id = appUser.Id,
                    Username = appUser.UserName,
                    Email = appUser.Email,
                    Role =role.FirstOrDefault(),
                    IsActive=appUser.IsActive
                };
                allUserViewModels.Add(allUserViewModel);
            }
            return View(allUserViewModels);
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UserBlock(string id)
        {
            var user=await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            if (!user.IsActive)
                return BadRequest();

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UserUnblock(string id)
        {
            var user=await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();
            if (user.IsActive)
                return BadRequest();

            user.IsActive = true;
            var result =await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));

        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            if (user is null)
                return NotFound();

            ChangeRoleUserViewModel changeRoleUserViewModel = new()
            {
                Username = user.UserName,
                Role = roles.FirstOrDefault()
            };
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(changeRoleUserViewModel);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(string id,ChangeRoleUserViewModel changeRoleUserViewModel)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            if (user is null)
                return NotFound();

            var role=roles.FirstOrDefault();
            if (role == RoleType.Admin.ToString())
                return BadRequest();

        
            await _userManager.RemoveFromRoleAsync(user, role);
            await _userManager.AddToRoleAsync(user, changeRoleUserViewModel.Role);
            return RedirectToAction(nameof(Index));

        }
    }
}
