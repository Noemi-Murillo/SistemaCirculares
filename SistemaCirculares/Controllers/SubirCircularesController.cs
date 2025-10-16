using Microsoft.AspNetCore.Mvc;

namespace SistemaCirculares.Controllers
{
    public class SubirCircularesController : Controller
    {
        public IActionResult Publicarcircular()
        {
            return View();
        }
    }
}
