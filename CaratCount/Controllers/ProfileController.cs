using System.Security.Claims;
using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;
        public ProfileController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }


        // GET: /profile
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Index()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users
                .Include(u => u.GstInDetail)
                .ThenInclude(g => g.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel? profileViewModel = new()
            {
                UserId = user.Id,
                GstInDetailId = user?.GstInDetail?.Id ?? Guid.NewGuid(),
                AddressId = user?.GstInDetail?.Address?.Id ?? Guid.NewGuid(),
                UserName = user.UserName,
                UserEmail = user.Email,
                PhoneNumber = user.PhoneNumber,
                GstInNo = user.GstInNo,
                LegalName = user?.GstInDetail?.LegalName,
                TradeName = user?.GstInDetail?.TradeName,
                BuildingName = user?.GstInDetail?.Address?.BuildingName,
                StreetName = user?.GstInDetail?.Address?.StreetName,
                FloorNumber = user?.GstInDetail?.Address?.FloorNumber,
                UnitNumber = user?.GstInDetail?.Address?.UnitNumber,
                Locality = user?.GstInDetail?.Address?.Locality,
                City = user?.GstInDetail?.Address?.City,
                District = user?.GstInDetail?.Address?.District,
                State = user?.GstInDetail?.Address?.State,
                Country = user?.GstInDetail?.Address?.Country,
                PostalCode = user?.GstInDetail?.Address?.PostalCode,

            };

            ViewBag.PageName = "Profile";
            return View(profileViewModel);
        }

        // GET: /profile/edit
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Edit()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users
                .Include(u => u.GstInDetail)
                .ThenInclude(g => g.Address)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            ProfileViewModel? profileViewModel = new()
            {
                UserId = user.Id,
                GstInDetailId = user?.GstInDetail?.Id ?? Guid.NewGuid(),
                AddressId = user?.GstInDetail?.Address?.Id ?? Guid.NewGuid(),
                UserName = user.UserName,
                UserEmail = user.Email,
                PhoneNumber = user.PhoneNumber,
                GstInNo = user.GstInNo,
                LegalName = user?.GstInDetail?.LegalName,
                TradeName = user?.GstInDetail?.TradeName,
                BuildingName = user?.GstInDetail?.Address?.BuildingName,
                StreetName = user?.GstInDetail?.Address?.StreetName,
                FloorNumber = user?.GstInDetail?.Address?.FloorNumber,
                UnitNumber = user?.GstInDetail?.Address?.UnitNumber,
                Locality = user?.GstInDetail?.Address?.Locality,
                City = user?.GstInDetail?.Address?.City,
                District = user?.GstInDetail?.Address?.District,
                State = user?.GstInDetail?.Address?.State,
                Country = user?.GstInDetail?.Address?.Country,
                PostalCode = user?.GstInDetail?.Address?.PostalCode,

            };

            ViewBag.PageName = "Profile";

            return View(profileViewModel);
        }

        // POST: /profile/edit
        [HttpPost]
        [Authorize()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileViewModel profileViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _context.Users
                    .Include(u => u.GstInDetail)
                        .ThenInclude(g => g.Address)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return NotFound();
                }

                var existingUser = await _userManager.FindByNameAsync(profileViewModel.UserName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    ModelState.AddModelError("UserName", "Username is already taken.");
                   
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;

                    return View(profileViewModel);
                }

                user.UserName = profileViewModel.UserName;
                user.Email = profileViewModel.UserEmail;
                user.PhoneNumber = profileViewModel.PhoneNumber;
                user.GstInNo = profileViewModel.GstInNo;

                if (user.GstInDetail != null)
                {
                    user.GstInDetail.GstInNo = profileViewModel.GstInNo;
                    user.GstInDetail.LegalName = profileViewModel.LegalName;
                    user.GstInDetail.TradeName = profileViewModel.TradeName;

                    user.GstInDetail.Address.BuildingName = profileViewModel.BuildingName;
                    user.GstInDetail.Address.StreetName = profileViewModel.StreetName;
                    user.GstInDetail.Address.FloorNumber = profileViewModel.FloorNumber;
                    user.GstInDetail.Address.UnitNumber = profileViewModel.UnitNumber;
                    user.GstInDetail.Address.Locality = profileViewModel.Locality;
                    user.GstInDetail.Address.City = profileViewModel.City;
                    user.GstInDetail.Address.District = profileViewModel.District;
                    user.GstInDetail.Address.State = profileViewModel.State;
                    user.GstInDetail.Address.Country = profileViewModel.Country;
                    user.GstInDetail.Address.PostalCode = profileViewModel.PostalCode;
                }
                else
                {
                    var address = new Address
                    {
                        BuildingName = profileViewModel.BuildingName,
                        StreetName = profileViewModel.StreetName,
                        FloorNumber = profileViewModel.FloorNumber,
                        UnitNumber = profileViewModel.UnitNumber,
                        Locality = profileViewModel.Locality,
                        City = profileViewModel.City,
                        District = profileViewModel.District,
                        State = profileViewModel.State,
                        Country = profileViewModel.Country,
                        PostalCode = profileViewModel.PostalCode
                    };

                    _context.Addresses.Add(address);

                    var gstInDetail = new GstInDetail
                    {
                        UserId = userId,
                        GstInNo = profileViewModel.GstInNo,
                        LegalName = profileViewModel.LegalName,
                        TradeName = profileViewModel.TradeName,
                        Address = address
                    };

                    _context.GstInDetails.Add(gstInDetail);

                    user.GstInDetailId = gstInDetail.Id;
                }

                try
                {
                    await _context.SaveChangesAsync();

                    TempData["ToastMessage"] = "Profile edited successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Profile");
                }
                catch 
                {
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return RedirectToAction("Edit");
                }
            }

            TempData["ToastMessage"] = "Something went wrong! Please try again.";
            TempData["ToastStatus"] = ToastStatus.Danger;

            ViewBag.PageName = "Profile";
            return View(profileViewModel);
        }

  
    }
}
