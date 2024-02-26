using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_125.Models;
using GBC_Travel_Group_125.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GBC_Travel_Group_125.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vehicles
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vehicles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VehicleId,VehicleName,VehicleType,Location,PhoneNumber,Model,Color,MaxCapacity,Price,Availability,VehicleDescription")] Vehicles vehicle, IFormFile? vehicleImage)
        {
            if (ModelState.IsValid)
            {
                var uploadedImagePath = await ProcessVehicleImageUpload(vehicleImage);
                vehicle.VehicleImage = uploadedImagePath ?? "wwwroot/images";


                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vehicle);
        }






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

        [HttpPost]
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
                        vehicleToUpdate.VehicleImage = uploadedImagePath; // Update with the new image path
                    }


                    // Update other properties
                    vehicleToUpdate.VehicleName = vehicle.VehicleName;
                    vehicleToUpdate.VehicleType = vehicle.VehicleType;
                    vehicleToUpdate.Location = vehicle.Location;
                    vehicleToUpdate.PhoneNumber = vehicle.PhoneNumber;
                    vehicleToUpdate.Model = vehicle.Model;
                    vehicleToUpdate.Color = vehicle.Color;
                    vehicleToUpdate.MaxCapacity = vehicle.MaxCapacity;
                    vehicleToUpdate.Price = vehicle.Price;
                    vehicleToUpdate.Availability = vehicle.Availability;
                    vehicleToUpdate.VehicleDescription = vehicle.VehicleDescription;

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




    }
}