using System.Security.Claims;
using CaratCount.Data;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Managers;
using CaratCount.Migrations;
using CaratCount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CaratCount.Controllers
{
    public class ClientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IClientManager _clientManager;
        public ClientController(UserManager<ApplicationUser> userManager, IClientManager clientManager)
        {
            _userManager = userManager;
            _clientManager = clientManager;
        }

        // GET: /client
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
           
            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Client>? clients = _clientManager.GetClientsByUserId(user.Id);

            ViewBag.PageName = "Client";

            return View(clients);
        }

        // GET: /client/add
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Add()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            ClientViewModel? clientViewModel = new () { UserId = user.Id };

            ViewBag.PageName = "Client";
            ViewBag.PageAction = "Add";
            return View("Edit", clientViewModel);
        }

        // GET: /client/details/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {

            string? userId = _userManager.GetUserId(User);

            if (userId == null || string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Client? client = await _clientManager.GetClientByIdAsync(id, userId);

            if(client == null) return NotFound();

            ClientViewModel? clientViewModel = new() { UserId = userId,

                ClientId = client.Id,
                GstInDetailId = client.GstInDetailId,
                AddressId = client.GstInDetail?.AddressId ?? Guid.NewGuid(),
                Name = client.Name,                                                   
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                GstInNo = client?.GstInDetail?.GstInNo,
                LegalName = client?.GstInDetail?.LegalName,
                TradeName = client?.GstInDetail?.TradeName,
                BuildingName = client?.GstInDetail?.Address?.BuildingName,
                StreetName = client?.GstInDetail?.Address?.StreetName,
                FloorNumber = client?.GstInDetail?.Address?.FloorNumber,
                UnitNumber = client?.GstInDetail?.Address?.UnitNumber,
                Locality = client?.GstInDetail?.Address?.Locality,
                City = client?.GstInDetail?.Address?.City,
                District = client?.GstInDetail?.Address?.District,
                State = client?.GstInDetail?.Address?.State,
                Country = client?.GstInDetail?.Address?.Country,
                PostalCode = client?.GstInDetail?.Address?.PostalCode ,
                DiamondPackets = client?.DiamondPackets
            };

            ViewBag.PageName = "Client";

            return View(clientViewModel);
        }

        // GET: /client/edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return RedirectToAction("Index", "Client");
            }

            Client? client = await _clientManager.GetClientByIdAsync(id, userId);

            if (client == null || client?.UserId != userId)
            {
                return NotFound();
            }

            ClientViewModel clientViewModel = new ()
            {
                UserId = userId,
                ClientId = client.Id,
                Name = client.Name,
                Email = client?.Email,
                GstInDetailId = client?.GstInDetail?.Id ?? Guid.NewGuid(),
                AddressId = client?.GstInDetail?.Address?.Id ?? Guid.NewGuid(),
                PhoneNumber = client?.PhoneNumber,
                GstInNo = client?.GstInDetail?.GstInNo,
                LegalName = client?.GstInDetail?.LegalName,
                TradeName = client?.GstInDetail?.TradeName,
                BuildingName = client?.GstInDetail?.Address?.BuildingName,
                StreetName =    client?.GstInDetail?.Address?.StreetName,
                FloorNumber = client?.GstInDetail?.Address?.FloorNumber,
                UnitNumber = client?.GstInDetail?.Address?.UnitNumber,
                Locality =  client?.GstInDetail?.Address?.Locality,
                City = client?.GstInDetail?.Address?.City,
                District = client?.GstInDetail?.Address?.District,
                State = client?.GstInDetail?.Address?.State,
                Country = client?.GstInDetail?.Address?.Country,
                PostalCode = client?.GstInDetail?.Address?.PostalCode,
            };

            ViewBag.PageName = "Client";
            ViewBag.PageAction = "Edit";

            return View(clientViewModel);
        }

       

        // GET: /client/delete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            string? userId = _userManager.GetUserId(User);

            if (userId == null)
            {
                return NotFound();
            }

            Client? client = await _clientManager.GetClientByIdAsync(id, userId);

            if (client == null || client?.UserId != userId)
            {
                return NotFound();
            }

            TempData["ModalAction"] = "Delete";
            TempData["ModalController"] = "Client";
            TempData["ModalMessage"] = $"Are you sure you want to delete {client.Name}?";
            TempData["ModalTitle"] = "Confirm Delete";
            TempData["ModalId"] = client.Id;
            TempData.Keep();

            return RedirectToAction("Index", "Client");
        }

        // POST: /client/add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ClientViewModel clientViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string userId = _userManager.GetUserId(User);

                    await _clientManager.AddClientAsync(clientViewModel, userId);

                    TempData["ToastMessage"] = "Client added successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Client");
                }

            
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

            ViewBag.PageName = "Client";
            ViewBag.PageAction = "Add";
            return View("Edit",clientViewModel);
            
        }


        // POST: /client/edit/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ClientViewModel clientViewModel)
        {
            Guid clientId;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out clientId)) 
            {
                return NotFound();
            }

            ViewBag.PageName = "Client";
            ViewBag.PageAction = "Edit";

            if (ModelState.IsValid)
            {
                try
                {
                    string userId = _userManager.GetUserId(User);
                  
                    Client? existingClient = await _clientManager.GetClientByIdAsync(id, userId);

                    if (existingClient == null)
                    {
                        return NotFound();
                    }

                   
                   
                    await _clientManager.UpdateClientAsync(clientViewModel, userId);

                    TempData["ToastMessage"] = "Client updated successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Client");
                }
                catch
                {
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return View(clientViewModel);
                }
            }

        
            return View(clientViewModel);
        }

        // POST: /client/delete/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool confirm)
        {
            if (!confirm)
            {
                return RedirectToAction("Index", "Client");
            }

            Guid clientId;

            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out clientId))
            {
                return NotFound();
            }


            try
            {
                string userId = _userManager.GetUserId(User);

                Client? client = await _clientManager.GetClientByIdAsync(id, userId);

                if (client == null)
                {
                    return NotFound();
                }


                await _clientManager.DeleteClientAsync(client);


                TempData["ToastMessage"] = "Client deleted successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;
                TempData.Keep();

                return RedirectToAction("Index", "Client");
            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Index", "Client");
            }
        
        }
    }
}
