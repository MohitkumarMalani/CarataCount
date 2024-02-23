using CaratCount.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CaratCount.Controllers
{
    public class DashboardController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;
        public DashboardController(UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        // GET: /dashboard
        [HttpGet]
        [Authorize()]
        public IActionResult Index()
        {
            
            return View();
        }

        
    }
}
