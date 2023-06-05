using Back_End_Project.Areas.Admin.ViewModels;
using Back_End_Project.Contexts;
using Back_End_Project.Models;
using Back_End_Project.Utilits;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_Project.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager,SignInManager<AppUser> signInManager,IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
          _roleManager = roleManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
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
                Email = userViewModel.Email,
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


            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string? url = Url.Action("activateUser", "Account", new { userId = user.Id, token }, HttpContext.Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();

            string body = await GetEmailTemplateAsync(url, "activateUser.html");

            MailRequestViewModel mailRequestViewModel = new()
            {
                ToEmail = user.Email,
                Subject = "Active your account",
                Body = body
            };

            await emailHelper.SendEmailAsync(mailRequestViewModel);

            return RedirectToAction(nameof(EmailVerification));
        }
        public async Task<IActionResult> EmailVerification()
        {
            return View();
        }
        
        public async Task<IActionResult> activateUser(string userId,string token)
        {
            //if(string.IsNullOrWhiteSpace(userId)||string.IsNullOrWhiteSpace(token)) { return BadRequest(); }
            var user = await _userManager.FindByIdAsync(userId);
            user.IsActive = true;
            await _userManager.UpdateAsync(user);

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


        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {

            if (!ModelState.IsValid)
                return View();
            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if (user is null)
                return NotFound();

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string? url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, HttpContext.Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();

            string body = await GetEmailTemplateAsync(url,"sendLink.html");

            MailRequestViewModel mailRequestViewModel = new()
            {
                ToEmail = user.Email,
                Subject = "Reset your password",
                Body = body
            };

            await emailHelper.SendEmailAsync(mailRequestViewModel);

            return RedirectToAction(nameof(Login));
        }
        private async Task<string> GetEmailTemplateAsync(string url,string filename)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "admin", filename);

            using StreamReader streamReader = new StreamReader(path);
            string result = await streamReader.ReadToEndAsync();

            result = result.Replace("[reset_password_url]", url);
            streamReader.Close();
            return result;
        }
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordViewModel.UserId) || string.IsNullOrWhiteSpace(resetPasswordViewModel.Token))
                return BadRequest();

            var user = await _userManager.FindByIdAsync(resetPasswordViewModel.UserId);
            if (user is null)
                return NotFound();

            ViewBag.UserName = user.UserName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ChangePassword(string userId,string token,ChangePasswordViewModel changePasswordViewModel)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound();

            ViewBag.UserName = user.UserName;
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(ResetPassword));



            await _userManager.ResetPasswordAsync(user, token, changePasswordViewModel.Password);
            return RedirectToAction(nameof(Login));





        }


    }
    
}
