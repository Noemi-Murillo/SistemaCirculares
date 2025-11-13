using Microsoft.AspNetCore.Mvc;

namespace API_Circulares.Controllers
{
    public class HomeAPIController : Controller
    {
        public IActionResult API_Circulares()
        {
            return View();
        }
    }
}
