using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GBC_Travel_Group_125.Data;
using GBC_Travel_Group_125.Models;

namespace GBC_Travel_Group_125.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User); // Get current user's ID
            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.User) // Include this if you want to display user details
                .Include(b => b.Vehicle) // Make sure you have a navigation property for Vehicle in your Booking model
                .ToListAsync();

            return View(bookings);
        }

    }
}
