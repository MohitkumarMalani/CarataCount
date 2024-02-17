using CaratCount.Entities;
using CaratCount.Models;
using CaratCount.Models.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaratCount.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<HomeController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: /account/register
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.PageName = "Register";
            var model = new RegisterViewModel();
            return View(model);
        }

        // POST: /account/register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.UserEmail, PhoneNumber = model.PhoneNumber,GstInNo = model.GstInNo };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            ViewBag.PageName = "Register";
            TempData["ToastMessage"] = "An error occurred while registering.";
            TempData["ToastStatus"] = ToastStatus.Danger;
            return View(model);
        }
        // GET: /account/login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.PageName = "Login";
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password,
                   isPersistent: model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        // if the logged-in user is an admin
                        var user = await _userManager.FindByNameAsync(model.UserName);
                        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                        if (isAdmin)
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }
            }

                ViewBag.PageName = "Login";
            TempData["ToastMessage"] = "Invalid username/password.";
            TempData["ToastStatus"] = ToastStatus.Danger;
            return View(model);
        }


        // POST: /account/logout
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        // Get: /account/access-denied
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewBag.PageName = "AccessDenied";
            return View();
        }
    }
}
