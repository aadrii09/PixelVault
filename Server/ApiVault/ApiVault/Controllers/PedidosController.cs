using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    public class PedidosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
