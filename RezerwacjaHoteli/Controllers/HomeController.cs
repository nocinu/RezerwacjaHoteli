using Microsoft.AspNetCore.Mvc;
using RezerwacjaHoteli.Data;
using System.Diagnostics;

namespace RezerwacjaHoteli.Controllers
{
    public class HomeController : Controller
    {
        private readonly HotelReservationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(HotelReservationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var hotels = _context.Hotels.Where(h => h.IsActive).ToList();

            return View(hotels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
