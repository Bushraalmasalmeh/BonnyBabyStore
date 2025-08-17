using Microsoft.AspNetCore.Mvc;

namespace BonnyBabyStore.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
