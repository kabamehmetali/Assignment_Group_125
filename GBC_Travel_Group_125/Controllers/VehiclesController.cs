using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_125.Models;
using GBC_Travel_Group_125.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace GBC_Travel_Group_125.Controllers
{
    [Authorize] // Ensures that only authenticated users can access methods in this controller.
    public class VehiclesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Vehicles
        [AllowAnonymous] // This allows unauthorized users to view the listing.
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 8; // Set page size to 8 items per page
            var vehicles = await _context.Vehicles
                                         .Skip((page - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();

            // Total number of items in the database
            int totalItems = await _context.Vehicles.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(vehicles);
        }

        // GET: Vehicles/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,VehicleName,VehicleType,Location,PhoneNumber,Model,Color,MaxCapacity,Price,Availability,VehicleDescription")] Vehicles vehicle, IFormFile? vehicleImage)
        {
            if (ModelState.IsValid)
            {
                var uploadedImagePath = await ProcessVehicleImageUpload(vehicleImage);
                vehicle.VehicleImage = uploadedImagePath ?? "wwwroot/images/defaultVehicle.jpg"; // Ensure you have a default image at this path

                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VehicleId,VehicleName,VehicleType,Location,PhoneNumber,Model,Color,MaxCapacity,Price,Availability,VehicleDescription")] Vehicles vehicle, IFormFile? newVehicleImage)
        {
            if (id != vehicle.VehicleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vehicleToUpdate = await _context.Vehicles.FindAsync(id);

                    if (vehicleToUpdate == null)
                    {
                        return NotFound();
                    }

                    if (newVehicleImage != null && newVehicleImage.Length > 0)
                    {
                        var uploadedImagePath = await ProcessVehicleImageUpload(newVehicleImage);
                        vehicleToUpdate.VehicleImage = uploadedImagePath;
                    }

                    _context.Update(vehicleToUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.VehicleId))
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
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.VehicleId == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            _context.Vehicles.Remove(vehicle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleExists(int id)
        {
            return _context.Vehicles.Any(e => e.VehicleId == id);
        }

        private async Task<string?> ProcessVehicleImageUpload(IFormFile? file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(uploadPath, fileName);

                try
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return "/images/" + fileName;
                }
                catch
                {
                    // Handle exceptions (e.g., logging) here
                }
            }
            return null; // Return null if no file was uploaded or if there was an error
        }
        public async Task<IActionResult> Search(string location, bool? availability, int? minCapacity, int? maxCapacity, string model, int page = 1)
        {
            int pageSize = 8; // Set page size to 8 items per page

            var vehiclesQuery = _context.Vehicles.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.Location.Contains(location));
            }
            if (availability.HasValue)
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.Availability == availability.Value);
            }
            else
            {
                // If availability filter is not specified, default to showing only available vehicles
                vehiclesQuery = vehiclesQuery.Where(v => v.Availability == true);
            }
            if (minCapacity.HasValue)
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.MaxCapacity >= minCapacity.Value);
            }
            if (maxCapacity.HasValue)
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.MaxCapacity <= maxCapacity.Value);
            }
            if (!string.IsNullOrEmpty(model))
            {
                vehiclesQuery = vehiclesQuery.Where(v => v.Model.Contains(model));
            }

            int totalItems = await vehiclesQuery.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["SearchCriteria"] = new { Location = location, Availability = availability, MinCapacity = minCapacity, MaxCapacity = maxCapacity, Model = model };

            var filteredVehicles = await vehiclesQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return View("Search", filteredVehicles);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Book(int id, DateTime startDate, DateTime endDate)
        {
            // Check vehicle existence
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle == null)
            {
                return NotFound("Vehicle not found.");
            }

            // Check for past start date
            if (startDate.Date < DateTime.UtcNow.Date)
            {
                ModelState.AddModelError("", "Booking cannot start in the past.");
                return View("Details", vehicle); // Assume Details is the view where booking is initiated
            }

            // Check if end date is before start date
            if (endDate <= startDate)
            {
                ModelState.AddModelError("", "End date must be after the start date.");
                return View("Details", vehicle);
            }

            // Fetch the current user
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge(); // Prompts the user to login if somehow they aren't.
            }

            // Calculate the total cost based on vehicle price and the booking duration
            decimal totalCost = (decimal)((endDate - startDate).TotalHours * vehicle.Price);

            // Check if user has enough balance
            if (user.Balance < totalCost)
            {
                ViewData["ErrorMessage"] = "Insufficient balance to complete the booking.";
                return View("Details", vehicle);
            }

            // Create and prepare the booking entry
            var booking = new Booking
            {
                VehicleId = id,
                UserId = user.Id,
                StartDate = startDate,
                EndDate = endDate,
                BookingDate = DateTime.UtcNow, // Use UTC for consistency in date/time records
                ServiceType = vehicle.VehicleType // Optional: storing vehicle type in booking
            };

            // Update user balance
            user.Balance -= totalCost;
            _context.Update(user);

            // Add the booking to the context
            _context.Bookings.Add(booking);
            try
            {
                // Save changes to the database
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Bookings"); // Redirect to the bookings index page on success
            }
            catch (DbUpdateException ex)
            {
                // Log the exception here, for example using a logging framework or storing the error in a database
                ModelState.AddModelError("", "An error occurred saving the booking. Please try again.");
                return View("Details", vehicle);
            }
        }







    }




}
