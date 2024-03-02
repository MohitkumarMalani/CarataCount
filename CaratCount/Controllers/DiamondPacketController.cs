using System.Net.Sockets;
using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Managers;
using CaratCount.Migrations;
using CaratCount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaratCount.Controllers
{
    public class DiamondPacketController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDiamondPacketManager _diamondPacketManager;
        private readonly IClientManager _clientManager;

        public DiamondPacketController(UserManager<ApplicationUser> userManager, IDiamondPacketManager diamondPacketManager, IClientManager clientManager)
        {
            _userManager = userManager;
            _diamondPacketManager = diamondPacketManager;
            _clientManager = clientManager;
        }

        // GET: /diamond-packet
        [HttpGet("diamond-packet")]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<DiamondPacket>? diamondPackets = await _diamondPacketManager
                .GetDiamondPacketsByUserIdAsync(user.Id);

            ViewBag.PageName = "DiamondPacket";

            return View(diamondPackets);
        }

        // GET: /diamond-packet/add
        [HttpGet("diamond-packet/add")]
        [Authorize()]
        public async Task<IActionResult> Add()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            DiamondPacket? diamondPacket = new () { UserId = user.Id };

            ViewData["Clients"] = _clientManager.GetClientsByUserId(user.Id);

            ViewBag.PageName = "DiamondPacket";
            ViewBag.PageAction = "Add";
            return View("Edit", diamondPacket);
        }

        // GET: /diamond-packet/details/{id}
        [HttpGet("diamond-packet/details/{id}")]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            DiamondPacket? diamondPacket = await _diamondPacketManager.GetDiamondPacketByIdAsync(id, user.Id);

            if (diamondPacket == null) return NotFound();

            ViewBag.PageName = "DiamondPacket";

            return View(diamondPacket);
        }

        // GET: /diamond-packet/edit/{id}
        [HttpGet("diamond-packet/edit/{id}")]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "DiamondPacket");
            }

            DiamondPacket? diamondPacket = await _diamondPacketManager.GetDiamondPacketByIdAsync(id, user.Id);

            if(diamondPacket == null) 
            { 
                return NotFound(); 
            }

            ViewData["Clients"] = _clientManager.GetClientsByUserId(user.Id);
            ViewBag.PageName = "DiamondPacket";
            ViewBag.PageAction = "Edit";

            return View(diamondPacket);
        }

        // GET: /diamond-packet/delete/{id}
        [HttpGet("diamond-packet/delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Index", "DiamondPacket");
            }

            DiamondPacket? diamondPacket = await _diamondPacketManager.GetDiamondPacketByIdAsync(id, user.Id);

            if (diamondPacket == null) 
            { 
                return NotFound(); 
            }

            TempData["ModalAction"] = "Delete";
            TempData["ModalController"] = "DiamondPacket";
            TempData["ModalMessage"] = $"Are you sure you want to delete diamondPacket with id: {diamondPacket.Id}?";
            TempData["ModalTitle"] = "Confirm Delete";
            TempData["ModalId"] = diamondPacket.Id;
            TempData.Keep();

            return RedirectToAction("Index", "DiamondPacket");
        }

        // POST: /diamond-packet/add
        [HttpPost("diamond-packet/add")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(DiamondPacket diamondPacket)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                if (ModelState.IsValid)
                {
                   
                    if (user == null || user.Id != diamondPacket.UserId)
                    {
                        return NotFound();
                    }

                    Client? client = await _clientManager.GetClientByIdAsync(diamondPacket.ClientId.ToString(), user.Id);
                    if (client == null) { return NotFound(); }

                    await _diamondPacketManager.AddDiamondPacketAsync(diamondPacket);

                    TempData["ToastMessage"] = "Diamond Packet added successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "DiamondPacket");
                }

                       ViewData["Clients"] = _clientManager.GetClientsByUserId(user.Id);
            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Add");
            }

            TempData["ToastMessage"] = "Something went wrong! Please try again.";
            TempData["ToastStatus"] = ToastStatus.Danger;

           
            ViewBag.PageName = "DiamondPacket";
            ViewBag.PageAction = "Add";
            return View("Edit", diamondPacket);

        }


        // POST: /diamond-packet/edit/{id}
        [HttpPost("diamond-packet/edit/{id}")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DiamondPacket diamondPacket)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            ViewData["Clients"] = _clientManager.GetClientsByUserId(user.Id);
            ViewBag.PageName = "DiamondPacket";
            ViewBag.PageAction = "Edit";

            if (ModelState.IsValid)
            {
                try
                {

                    if (diamondPacket.UserId != user.Id)
                    {
                        return NotFound();
                    }

                    await _diamondPacketManager.UpdateDiamondPacketAsync(diamondPacket);

                    TempData["ToastMessage"] = "Diamond packet updated successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "DiamondPacket");
                }
                catch
                {
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return View(diamondPacket);
                }
            }


            return View(diamondPacket);
        }

        // POST: /client/delete/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool confirm)
        {
            if (!confirm)
            {
                return RedirectToAction("Index", "DiamondPacket");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);


            try
            {

                DiamondPacket? diamondPacket = await _diamondPacketManager.GetDiamondPacketByIdAsync(id, user.Id);

                if (diamondPacket == null)
                {
                    return NotFound();
                }


                await _diamondPacketManager.DeleteDiamondPacketAsync(diamondPacket);


                TempData["ToastMessage"] = "Diamond packet deleted successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;
                TempData.Keep();

                return RedirectToAction("Index", "DiamondPacket");
            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Index", "DiamondPacket");
            }

        }
    }
}
