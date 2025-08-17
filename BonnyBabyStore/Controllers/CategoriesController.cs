using Microsoft.AspNetCore.Mvc;

namespace BonnyBabyStore.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
