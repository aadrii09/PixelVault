using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    public class CarritosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
