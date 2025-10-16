using Microsoft.AspNetCore.Mvc;

namespace SistemaCirculares.Controllers
{
    public class ComitesController : Controller
    {
        public IActionResult GestionComites()
        {
            return View();
        }
    }
}
