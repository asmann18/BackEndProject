using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Utilits;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_Project.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
          _roleManager = roleManager;
            _signInManager = signInManager;
        }

       

        public async Task<IActionResult> Register()
        {
          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
   
            if (User.Identity.IsAuthenticated)
                return BadRequest();
            if (!ModelState.IsValid)
                return View();
            AppUser user = new()
            {
                UserName = userViewModel.Username,
                Fullname = userViewModel.Fullname,
                Email = userViewModel.Email
            };
       

            var identity = await _userManager.CreateAsync(user, userViewModel.Password);
            if (!identity.Succeeded)
            {
                foreach (var error in identity.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }
            await _userManager.AddToRoleAsync(user, RoleType.Member.ToString());
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();
            var user = await _userManager.FindByNameAsync(loginViewModel.Username);
            if (user is null)
            {
                ModelState.AddModelError("", "Username or password invalid");
                return View();
            }
            if (!user.IsActive)
            {
                ModelState.AddModelError("", "User is blocked");
                return View();
            }


            var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
            
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account has been blocked, please check back later");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or password invalid");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return BadRequest();

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (var roleType in Enum.GetValues(typeof(RoleType)))
        //    {
        //        if (!await _roleManager.RoleExistsAsync(roleType.ToString()))
        //            await _roleManager.CreateAsync(new IdentityRole { Name = roleType.ToString() });
        //    }

        //    return RedirectToAction("Index","Home"); 
        //}
    }
}
