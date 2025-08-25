using Microsoft.AspNetCore.Mvc;

namespace SistemaCirculares.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Iniciosesion()
        {
            return View();
        }
    }
}
