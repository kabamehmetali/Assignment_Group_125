using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using GBC_Travel_Group_125.Data;
using GBC_Travel_Group_125.Models;

namespace GBC_Travel_Group_125.Controllers
{
    public class FlightsController : Controller
    {
        private readonly ApplicationDbContext _context; // Replace 'YourDbContext' with your actual DbContext class name


        public FlightsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            return View(await _context.Flights.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }
        public IActionResult Book(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = _context.Flights.FirstOrDefault(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost, ActionName("Book")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            // Implement your booking logic here
            // For example, add a booking record to the database

            // Redirect back to the Index page after booking
            return RedirectToAction("Index");
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Flights/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,FlightNumber,Airline,FlightFrom,FlightTo,DepartureDateAndTime,ArrivalDateAndTime,Capacity,Price")] Flights flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            return View(flight);
        }

        // POST: Flights/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,FlightNumber,Airline,FlightFrom,FlightTo,DepartureDateAndTime,ArrivalDateAndTime,Capacity,Price")] Flights flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }


        public async Task<IActionResult> Search(string flightNumber, string airline, string from, string to, DateTime? departureDate, int? capacity)
        {
            var flights = _context.Flights.AsQueryable();

            if (!string.IsNullOrEmpty(flightNumber))
            {
                flights = flights.Where(f => f.FlightNumber.Contains(flightNumber));
            }

            if (!string.IsNullOrEmpty(airline))
            {
                flights = flights.Where(f => f.Airline.Contains(airline));
            }

            if (!string.IsNullOrEmpty(from))
            {
                flights = flights.Where(f => f.FlightFrom.Contains(from));
            }

            if (!string.IsNullOrEmpty(to))
            {
                flights = flights.Where(f => f.FlightTo.Contains(to));
            }

            if (departureDate.HasValue)
            {
                flights = flights.Where(f => f.DepartureDateAndTime.Date == departureDate.Value.Date);
            }

            if (capacity.HasValue)
            {
                flights = flights.Where(f => f.Capacity >= capacity.Value);
            }

            return View("Index", await flights.ToListAsync());
        }
        public IActionResult Booking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = _context.Flights.FirstOrDefault(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }



       /* [HttpPost, ActionName("ConfirmBooking")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBookingPost(int id)
        {
            var flight = await _context.Flights.FindAsync(id);

            if (flight == null)
            {
                return NotFound();
            }

            // Assuming you have a Booking model
            var booking = new Booking
            {
                FlightNumber = flight.FlightNumber, // Assuming you want to store flight information in the booking
                BookingDate = DateTime.Now, // You can adjust this based on your requirements
                BookingType = 1, // Assuming a default booking type
                VehicleId = id // Assuming FlightId maps to VehicleId in Booking
            };

            _context.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
*/



    }
}

