using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        public AdminController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }


        // GET: /admin
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.GetUsersInRoleAsync("User");

            ViewBag.PageName = "Admin";
            return View(users);
        }

        // POST: /admin/block-user/id
        [HttpPost("/admin/block-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsBlocked = true;
                await _userManager.UpdateAsync(user);

                TempData["ToastMessage"] = "User blocked successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;
            }
            else
            { 
                TempData["ToastMessage"] = "Failed to block user.";
                TempData["ToastStatus"] = ToastStatus.Danger;
            }

            TempData.Keep();

            return RedirectToAction("Index");
        }

        // POST: /admin/unblock-user/id
        [HttpPost("/admin/unblock-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsBlocked = false;
                await _userManager.UpdateAsync(user);

                TempData["ToastMessage"] = "User unblocked successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;

            }
            else
            {
                TempData["ToastMessage"] = "Failed to unblock user.";
                TempData["ToastStatus"] = ToastStatus.Danger;
            }

            TempData.Keep();

            return RedirectToAction("Index");
        }
    }
}
