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
    public class EmployeeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmployeeManager _employeeManager;
        private readonly IDiamondPacketManager _diamondPacketManager;

        public EmployeeController(
            UserManager<ApplicationUser> userManager,
            IEmployeeManager employeeManager,
            IDiamondPacketManager diamondPacketManager)
        {
            _userManager = userManager;
            _employeeManager = employeeManager;
            _diamondPacketManager = diamondPacketManager;
        }

        // GET: /employee
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            List<Employee>? employees = await _employeeManager.GetEmployeesByUserIdAsync(user.Id);

            ViewBag.PageName = "Employee";

            return View(employees);
        }

        // GET: /employee/add
        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> Add()
        {

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            Employee? employee = new() { UserId = user.Id };


            ViewBag.PageName = "Employee";
            ViewBag.PageAction = "Add";
            return View("Edit", employee);
        }

        // GET: /employee/details/{id}
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

            Employee? employee = await _employeeManager.GetEmployeeByIdAsync(id, user.Id);

            if (employee == null) return NotFound();

            List<DiamondPacketProcess?> diamondPacketProcesses = await _diamondPacketManager.GetDiamondPacketProcessesByEmployeeByIdAsync(employee.Id.ToString());

            employee.DiamondPacketProcesses = diamondPacketProcesses;

            ViewBag.PageName = "Employee";

            return View(employee);
        }

        // GET: /employee/edit/{id}
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

            Employee? employee = await _employeeManager.GetEmployeeByIdAsync(id, user.Id);

            if (employee == null)
            {
                return NotFound();
            }

            ViewBag.PageName = "Employee";
            ViewBag.PageAction = "Edit";

            return View(employee);
        }

        // GET: /employee/delete/{id}
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

            Employee? employee = await _employeeManager.GetEmployeeByIdAsync(id, user.Id);

            if (employee == null)
            {
                return NotFound();
            }

            TempData["ModalAction"] = "Delete";
            TempData["ModalController"] = "Employee";
            TempData["ModalMessage"] = $"Are you sure you want to delete employee with id: {employee.Id}?";
            TempData["ModalTitle"] = "Confirm Delete";
            TempData["ModalId"] = employee.Id;
            TempData.Keep();

            return RedirectToAction("Index", "Employee");
        }

        // POST: /employee/add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Employee employee)
        {
            try
            {
                ApplicationUser? user = await _userManager.GetUserAsync(User);

                if (ModelState.IsValid)
                {

                    if (user == null || user.Id != employee.UserId)
                    {
                        return NotFound();
                    }

                    await _employeeManager.AddEmployeeAsync(employee);

                    TempData["ToastMessage"] = "Employee added successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Employee");
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


            ViewBag.PageName = "Employee";
            ViewBag.PageAction = "Add";
            return View("Edit", employee);

        }


        // POST: /employee/edit/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Employee employee)
        {

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            ViewBag.PageName = "Employee";
            ViewBag.PageAction = "Edit";

            if (ModelState.IsValid)
            {
                try
                {

                    if (employee.UserId != user.Id)
                    {
                        return NotFound();
                    }

                    await _employeeManager.UpdateEmployeeAsync(employee);

                    TempData["ToastMessage"] = "Employee updated successfully.";
                    TempData["ToastStatus"] = ToastStatus.Success;
                    TempData.Keep();

                    return RedirectToAction("Index", "Employee");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    TempData["ToastMessage"] = "Something went wrong! Please try again.";
                    TempData["ToastStatus"] = ToastStatus.Danger;
                    TempData.Keep();

                    return View(employee);
                }
            }


            return View(employee);
        }

        // POST: /employee/delete/{id}
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id, bool confirm)
        {
            if (!confirm)
            {
                return RedirectToAction("Index", "Employee");
            }

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            ApplicationUser? user = await _userManager.GetUserAsync(User);

            try
            {

                Employee? employee = await _employeeManager.GetEmployeeByIdAsync(id, user.Id);

                if (employee == null)
                {
                    return NotFound();
                }


                await _employeeManager.DeleteEmployeeAsync(employee);


                TempData["ToastMessage"] = "Employee deleted successfully.";
                TempData["ToastStatus"] = ToastStatus.Success;
                TempData.Keep();

                return RedirectToAction("Index", "Employee");
            }
            catch
            {
                TempData["ToastMessage"] = "Something went wrong! Please try again.";
                TempData["ToastStatus"] = ToastStatus.Danger;
                TempData.Keep();

                return RedirectToAction("Index", "Employee");
            }

        }
    }
}
