using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
    }
}
