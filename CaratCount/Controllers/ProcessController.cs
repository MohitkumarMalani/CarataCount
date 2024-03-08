using System.Diagnostics;
using CaratCount.Entities;
using CaratCount.Interface;
using CaratCount.Managers;
using CaratCount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Process = CaratCount.Entities.Process;

namespace CaratCount.Controllers
{
    public class ProcessController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IProcessManager _processManager;

        public ProcessController(UserManager<ApplicationUser> userManager, IEmployeeManager employeeManager, IProcessManager processManager)
        {
            _userManager = userManager;
            _employeeManager = employeeManager;
            _processManager = processManager;
        }

        // GET: /process
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Process>? processes = await _processManager.GetProcessesByUserIdAsync(user.Id);

            ViewBag.PageName = "Process";

            return View(processes);
        }

        // GET: /process/add
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Add()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            Process? process = new() { UserId = user.Id };


            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Add";
            return View("Edit", process);
        }

        // GET: /process/details/{id}
        [HttpGet]
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

            Process? process = await _processManager.GetProcessByIdAsync(id, user.Id);

            if (process == null) return NotFound();

            ViewBag.PageName = "Process";

            return View(process);
        }

        // GET: /process/edit/{id}
        [HttpGet]
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
                return NotFound();
            }

            Process? process = await _processManager.GetProcessByIdAsync(id, user.Id);

            if (process == null)
            {
                return NotFound();
            }

            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Edit";

            return View(process);
        }

        // GET: /process/delete/{id}
        [HttpGet]
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
                return NotFound();
            }

            Process? process = await _processManager.GetProcessByIdAsync(id, user.Id);

            if (process == null)
            {
                return NotFound();
            }

            TempData["ModalAction"] = "Delete";
            TempData["ModalController"] = "Process";
            TempData["ModalMessage"] = $"Are you sure you want to delete process with id: {process.Id}?";
            TempData["ModalTitle"] = "Confirm Delete";
            TempData["ModalId"] = process.Id;
            TempData.Keep();

            return RedirectToAction("Index", "Process");
        }

        // POST: /process/add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Process process)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                if (ModelState.IsValid)
                {

                    if (user == null || user.Id != process.UserId)
                    {
                        return NotFound();
                    }

                    await _processManager.AddProcessAsync(process);

                    TempData["ToastMessage"] = "Process added successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Process");
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


            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Add";
            return View("Edit", process);

        }


        // POST: /process/edit/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Process process)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Edit";

            if (ModelState.IsValid)
            {
                try
                {

                    if (process.UserId != user.Id)
                    {
                        return NotFound();
                    }

                    await _processManager.UpdateProcessAsync(process);

                    TempData["ToastMessage"] = "Process updated successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Process");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return View(process);
                }
            }


            return View(process);
        }

        // POST: /process/delete/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool confirm)
        {
            if (!confirm)
            {
                return RedirectToAction("Index", "Process");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            try
            {

                Process? process = await _processManager.GetProcessByIdAsync(id, user.Id);

                if (process == null)
                {
                    return NotFound();
                }


                await _processManager.DeleteProcessAsync(process);


                TempData["ToastMessage"] = "Process deleted successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;
                TempData.Keep();

                return RedirectToAction("Index", "Process");
            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Index", "Process");
            }

        }
    }
}
