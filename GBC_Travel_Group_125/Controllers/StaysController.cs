using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_125.Models;
using Microsoft.AspNetCore.Http;
using GBC_Travel_Group_125.Data;

namespace GBC_Travel_Group_125.Controllers
{
    public class StaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaysController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchTerm, string location, string hotelName, int? maxRoom)
        {
            var query = _context.Stays.AsQueryable();

            // Filter by search term
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(s => s.HotelName.Contains(searchTerm) || s.HotelLocation.Contains(searchTerm));
            }

            // Filter by location
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(s => s.HotelLocation.Contains(location));
            }

            // Filter by hotel name
            if (!string.IsNullOrEmpty(hotelName))
            {
                query = query.Where(s => s.HotelName.Contains(hotelName));
            }

            // Filter by maximum room
            if (maxRoom.HasValue)
            {
                query = query.Where(s => s.MaximumRoom >= maxRoom.Value);
            }

            var searchResults = await query.ToListAsync();
            return View(searchResults);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays
                .FirstOrDefaultAsync(m => m.StayId == id);
            if (stay == null)
            {
                return NotFound();
            }

            return View(stay);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StayId,HotelName,MaximumRoom,HotelLocation,PhoneNumber")] Stays stay, IFormFile hotelImageFile)
        {
            if (ModelState.IsValid)
            {
                if (hotelImageFile != null && hotelImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(hotelImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hotelImageFile.CopyToAsync(stream);
                    }

                    stay.HotelImage = "/images/" + fileName;
                }

                try
                {
                    _context.Add(stay);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, $"An error occurred while saving the stay: {ex.Message}");
                }
            }

            // If we're here, there's a validation error, and we want to preserve the file input
            return View(stay);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays.FindAsync(id);
            if (stay == null)
            {
                return NotFound();
            }
            return View(stay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StayId,HotelName,MaximumRoom,HotelLocation,PhoneNumber,HotelImage")] Stays stay)
        {
            if (id != stay.StayId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StayExists(stay.StayId))
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
            return View(stay);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = await _context.Stays
                .FirstOrDefaultAsync(m => m.StayId == id);
            if (stay == null)
            {
                return NotFound();
            }

            return View(stay);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stay = await _context.Stays.FindAsync(id);
            _context.Stays.Remove(stay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StayExists(int id)
        {
            return _context.Stays.Any(e => e.StayId == id);
        }

        public IActionResult Booking(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stay = _context.Stays.FirstOrDefault(m => m.StayId == id);
            if (stay == null)
            {
                return NotFound();
            }

            return View(stay);
        }
        [HttpPost, ActionName("ConfirmBookingPost")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBookingPost(int id)
        {
            var stay = await _context.Stays.FindAsync(id);

            if (stay == null)
            {
                return NotFound();
            }

            // Assuming you have a Booking model with a property named StayId
            var booking = new Booking
            {
                BookingId = stay.StayId, // Assuming you want to store stay information in the booking
                BookingDate = DateTime.Now, // You can adjust this based on your requirements
                BookingType = 1, // Assuming a default booking type
                                 // Other properties of Booking class
            };

            _context.Add(booking);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        private async Task<string> ProcessHotelImageUpload(IFormFile hotelImageFile)
        {
            if (hotelImageFile != null && hotelImageFile.Length > 0)
            {
                try
                {
                    // Saving the image file and returning the path
                    var fileName = Path.GetFileName(hotelImageFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await hotelImageFile.CopyToAsync(stream);
                    }

                    return "/images/" + fileName; // Adjust the path as needed
                }
                catch (Exception ex)
                {
                    // Handle exception if there's an issue with file upload
                    // Log the exception or display an error message
                    Console.WriteLine($"Error uploading hotel image: {ex.Message}");
                }
            }

            return null;
        }
    }
}
