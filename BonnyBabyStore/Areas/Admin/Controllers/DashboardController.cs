using Microsoft.AspNetCore.Mvc;
using BonnyBabyStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace BonnyBabyStore.Areas.Admin.Controllers
{
    [Area("Admin")]
     [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Reports()
        {
            ViewBag.TotalUsers = _context.Users.Count();
            ViewBag.TotalProducts = _context.Products.Count();
            ViewBag.TotalCategories = _context.Categories.Count();
     
            return View();
        }
    }
}
