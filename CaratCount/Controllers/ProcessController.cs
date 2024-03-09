using CaratCount.Entities;
using CaratCount.Interface;
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
        private readonly IProcessManager _processManager;

        public ProcessController(UserManager<ApplicationUser> userManager, IProcessManager processManager)
        {
            _userManager = userManager;
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

            ProcessViewModel? processViewModel = new() { UserId = user.Id };


            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Add";
            return View("Edit", processViewModel);
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

            decimal userCost = process.ProcessPrices?.Reverse()?.FirstOrDefault()?.UserCost ?? 0;
            decimal clientCharge = process.ProcessPrices?.Reverse().FirstOrDefault()?.ClientCharge ?? 0;

            ProcessViewModel? processViewModel = new()
            {
                UserId = user.Id,
                ProcessId = process.Id,
                Name = process.Name,
                Description = process.Description,
                UserCost = userCost,
                ClientCharge = clientCharge
            };

            ViewBag.PageName = "Process";
            ViewBag.PageAction = "Edit";

            return View(processViewModel);
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
        public async Task<IActionResult> Add(ProcessViewModel processViewModel)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                if (ModelState.IsValid)
                {

                    if (user == null || user.Id != processViewModel.UserId)
                    {
                        return NotFound();
                    }

                    await _processManager.AddProcessAsync(processViewModel);

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
            return View("Edit", processViewModel);

        }


        // POST: /process/edit/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ProcessViewModel processViewModel)
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

                    if (processViewModel.UserId != user.Id)
                    {
                        return NotFound();
                    }

                    await _processManager.UpdateProcessAsync(processViewModel);

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

                    return View(processViewModel);
                }
            }


            return View(processViewModel);
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
