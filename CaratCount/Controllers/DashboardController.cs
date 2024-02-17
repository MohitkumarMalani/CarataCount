using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CaratCount.Controllers
{
    public class DashboardController : Controller
    {
        // GET: /dashboard
        [HttpGet]
        [Authorize()]
        public IActionResult Index()
        {
            return View();
        }

        
    }
}
