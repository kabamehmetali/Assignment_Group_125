using GBC_Travel_Group_125.Data;
using GBC_Travel_Group_125.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GBC_Travel_Group_125.Controllers
{
  
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register() {
            return View();
        }
       
    }
}
