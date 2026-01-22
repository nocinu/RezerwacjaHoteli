using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjaHoteli.Data;
using RezerwacjaHoteli.Models;

namespace RezerwacjaHoteli.Controllers
{
    public class HotelsController : Controller
    {
        private readonly HotelReservationDbContext _context;

        public HotelsController(HotelReservationDbContext context)
        {
            _context = context;
        }

        // GET: Hotels
        public IActionResult Index()
        {
            var hotels = _context.Hotels.Where(h => h.IsActive).ToList();
            return View(hotels);
        }

        // GET: Hotels/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // załadowanie hotelu
            var hotel = _context.Hotels.FirstOrDefault(m => m.Id == id);
            if (hotel != null)
            {
                _context.Entry(hotel).Collection(h => h.Rooms).Load();


            }

            if (hotel == null)
            {
                return NotFound();
            }

            // załadowanie pokoji
            _context.Entry(hotel).Collection(h => h.Rooms).Load();

            return View(hotel);
        }



        // GET: Hotels/Create
        public IActionResult Create()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hotel hotel)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                hotel.CreatedDate = DateTime.Now;
                _context.Hotels.Add(hotel);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Edit/5
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

            var hotel = _context.Hotels.Find(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Hotel hotel)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            if (id != hotel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hotel);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hotel);
        }

        // GET: Hotels/Delete/5
        public IActionResult Delete(int? id)
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

            var hotel = _context.Hotels.FirstOrDefault(m => m.Id == id);
            if (hotel == null)
            {
                return NotFound();
            }

            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Admin")
            {
                return Forbid();
            }

            var hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                _context.Hotels.Remove(hotel);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
