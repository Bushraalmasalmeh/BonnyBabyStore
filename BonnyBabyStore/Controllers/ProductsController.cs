using Microsoft.AspNetCore.Mvc;

namespace BonnyBabyStore.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
