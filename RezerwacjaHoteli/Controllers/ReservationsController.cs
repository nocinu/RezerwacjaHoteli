using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RezerwacjaHoteli.Data;
using RezerwacjaHoteli.Models;

namespace RezerwacjaHoteli.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HotelReservationDbContext _context;

        public ReservationsController(HotelReservationDbContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userRole = HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            List<Reservation> reservations;

            if (userRole == "Admin")
            {
                // Admin widzi wszystkie rezerwacje
                reservations = _context.Reservations
                    .Include(r => r.Room)
                        .ThenInclude(r => r.Hotel)
                    .Include(r => r.User)
                    .OrderByDescending(r => r.CreatedDate)
                    .ToList();
            }
            else
            {
                // Guest widzi tylko swoje rezerwacje
                reservations = _context.Reservations
                    .Include(r => r.Room)
                        .ThenInclude(r => r.Hotel)
                    .Include(r => r.User)
                    .Where(r => r.UserId == int.Parse(userId))
                    .OrderByDescending(r => r.CreatedDate)
                    .ToList();
            }

            return View(reservations);
        }

        // GET: Reservations/Create
        [HttpGet]
        public IActionResult Create(int? roomId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Rooms = _context.Rooms
                .Include(r => r.Hotel)
                .OrderBy(r => r.Hotel.Name)
                .ToList();

            if (roomId.HasValue)
            {
                ViewBag.SelectedRoomId = roomId.Value;  
            }

            return View();
        }
        
        // POST: Reservations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int roomId, DateTime checkInDate, DateTime checkOutDate,
                                    int numberOfGuests, string? specialRequests = null)
        {
            // pusta wartość dla opcjonalnego pola
            specialRequests = specialRequests ?? "";
            ModelState.Remove("SpecialRequests");

            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var room = _context.Rooms.Find(roomId);

            // sprawdzamy czy pokój istnieje
            if (room == null)
            {
                ModelState.AddModelError("", "Pokój nie istnieje");
                ViewBag.Rooms = _context.Rooms
                    .Include(r => r.Hotel)
                    .OrderBy(r => r.Hotel.Name)
                    .ToList();
                return View();
            }

            // walidacja dat - wyjazd musi być później niż przyjazd
            if (checkOutDate <= checkInDate)
            {
                ModelState.AddModelError("", "Data wyjazdu musi być później niż data zaznaczenia");
                ViewBag.Rooms = _context.Rooms
                    .Include(r => r.Hotel)
                    .OrderBy(r => r.Hotel.Name)
                    .ToList();
                return View();
            }

            // data nie może być z przeszłości
            if (checkInDate < DateTime.Today)
            {
                ModelState.AddModelError("", "Nie możesz zarezerwować na datę przeszłą");
                ViewBag.Rooms = _context.Rooms
                    .Include(r => r.Hotel)
                    .OrderBy(r => r.Hotel.Name)
                    .ToList();
                return View();
            }

            // sprawdzamy czy w tym terminie coś już jest zarezerwowane
            var hasConflict = _context.Reservations.Any(r =>
                r.RoomId == roomId &&
                r.Status != "Cancelled" &&
                r.CheckOutDate > DateTime.Today &&
                (
                    (r.CheckInDate < checkOutDate && r.CheckOutDate > checkInDate)
                )
            );

            if (hasConflict)
            {
                ModelState.AddModelError("", "Pokój jest już zarezerwowany w wybranym terminie! Wybierz inne daty.");
                ViewBag.Rooms = _context.Rooms
                    .Include(r => r.Hotel)
                    .OrderBy(r => r.Hotel.Name)
                    .ToList();
                return View();
            }

            // walidacja liczby gości
            if (numberOfGuests > room.MaxGuests)
            {
                ModelState.AddModelError("", $"Maksymalna liczba gości to {room.MaxGuests}");
                ViewBag.Rooms = _context.Rooms
                    .Include(r => r.Hotel)
                    .OrderBy(r => r.Hotel.Name)
                    .ToList();
                return View();
            }

            // liczymy cenę za pobyt
            var nights = (checkOutDate - checkInDate).Days;
            var totalPrice = nights * room.PricePerNight;

            // tworzymy obiekt rezerwacji
            var reservation = new Reservation
            {
                UserId = int.Parse(userId),
                RoomId = roomId,
                CheckInDate = checkInDate,
                CheckOutDate = checkOutDate,
                NumberOfGuests = numberOfGuests,
                TotalPrice = totalPrice,
                Status = "Confirmed",
                SpecialRequests = specialRequests,
                CreatedDate = DateTime.Now
            };

            // zapis do bazy
            _context.Reservations.Add(reservation);
            _context.SaveChanges();



            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Cancel/5
        [HttpPost]
        public IActionResult Cancel(int id)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var reservation = _context.Reservations
                .Include(r => r.Room)
                .FirstOrDefault(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Sprawdzenie uprawnień
            if (reservation.UserId != int.Parse(userId) &&
                HttpContext.Session.GetString("UserRole") != "Admin")
            {
                return Forbid();
            }

            // Zmiana statusu rezerwacji na anulowaną
            reservation.Status = "Cancelled";



            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // sprawdzanie dostępności
        private bool IsRoomAvailableForDates(int roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var conflicts = _context.Reservations.Any(r =>
                r.RoomId == roomId &&
                r.Status != "Cancelled" &&
                r.CheckOutDate > DateTime.Today &&
                r.CheckInDate < checkOutDate &&
                r.CheckOutDate > checkInDate
            );

            return !conflicts;
        }
    }
}