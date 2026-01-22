using Microsoft.AspNetCore.Mvc;
using RezerwacjaHoteli.Data;
using RezerwacjaHoteli.Models;

namespace RezerwacjaHoteli.Controllers
{
    public class RoomsController : Controller
    {
        private readonly HotelReservationDbContext _context;

        public RoomsController(HotelReservationDbContext context)
        {
            _context = context;
        }

        // GET: Rooms/Create
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }
            ViewBag.Hotels = _context.Hotels.ToList();
            return View();
        }

        // POST: Rooms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            room.CreatedDate = DateTime.Now;
            room.Status = "Available";
            room.Description = room.Description ?? "Brak opisu";
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return RedirectToAction("Index", "Hotels");
        }

        // GET: Rooms/Edit/5
        public IActionResult Edit(int? id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            if (id == null)
            {
                return NotFound();
            }

            var room = _context.Rooms.Find(id);
            if (room == null)
            {
                return NotFound();
            }

            ViewBag.Hotels = _context.Hotels.ToList();
            return View(room);
        }

        // POST: Rooms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Room room)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(room);
                _context.SaveChanges();
                return RedirectToAction("Index", "Hotels");
            }

            ViewBag.Hotels = _context.Hotels.ToList();
            return View(room);
        }
    }
}
