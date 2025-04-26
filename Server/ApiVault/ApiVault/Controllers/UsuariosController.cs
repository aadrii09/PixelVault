using Microsoft.AspNetCore.Mvc;

namespace ApiVault.Controllers
{
    public class UsuariosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
