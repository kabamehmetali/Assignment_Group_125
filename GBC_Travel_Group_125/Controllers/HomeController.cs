using GBC_Travel_Group_125.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GBC_Travel_Group_125.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult VehicleSearch(string location, bool? availability, int? minCapacity, int? maxCapacity, string model, int page = 1)
        {
            // Redirect to the Search action in VehiclesController with the search parameters
            return RedirectToAction("Search", "Vehicles", new { location, availability, minCapacity, maxCapacity, model, page });
        }
    }
}
