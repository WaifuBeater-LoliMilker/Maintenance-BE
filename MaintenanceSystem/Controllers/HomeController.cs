using Microsoft.AspNetCore.Mvc;

namespace MaintenanceSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}