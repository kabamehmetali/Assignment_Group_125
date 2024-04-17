using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_125.Models; // Adjust the namespace to match your project
using Microsoft.AspNetCore.Http; // For IFormFile
using System.IO;
using GBC_Travel_Group_125.Data; // For file processing

namespace GBC_Travel_Group_125.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context; // Replace YourDbContext with your actual DbContext class

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Event.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,EventName,EventDescription,DateStart,DateEnd,Location,Capacity,TicketPrice,EventImage")] Events events, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    try
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", imageFile.FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                            events.EventImage = "/images/" + imageFile.FileName;
                        }
                    }
                    catch
                    {
                        // If image upload fails, assign a default text to EventImage
                        events.EventImage = "test.jpg";
                    }
                }
                else
                {
                    // If no image was uploaded, assign a default text to EventImage
                    events.EventImage = "test.jpg";
                }

                _context.Add(events);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(events);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event.FindAsync(id);
            if (events == null)
            {
                return NotFound();
            }
            return View(events);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,EventDescription,DateStart,DateEnd,Location,Capacity,TicketPrice,EventImage")] Events events)
        {
            if (id != events.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(events);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventsExists(events.EventId))
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
            return View(events);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var events = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var events = await _context.Event.FindAsync(id);
            _context.Event.Remove(events);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Events/Search
        public async Task<IActionResult> Search(string location, DateTime? date, int? tickets)
        {
            // Start with all events
            var query = _context.Event.AsQueryable();

            // Filter by location if provided
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            // Filter by date if provided
            if (date.HasValue)
            {
                query = query.Where(e => e.DateStart <= date.Value && e.DateEnd >= date.Value);
            }

            // Filter by the minimum number of available tickets if provided
            if (tickets.HasValue && tickets.Value > 0)
            {
                query = query.Where(e => e.Capacity >= tickets.Value);
            }

            var events = await query.ToListAsync();

            // Return a view (ensure you have a corresponding view named 'SearchResults' or change the view name as necessary)
            return View("_searchBar", events);
        }



        private bool EventsExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
    }
}
