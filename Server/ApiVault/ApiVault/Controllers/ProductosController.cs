using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
