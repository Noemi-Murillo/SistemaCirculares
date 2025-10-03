using Microsoft.AspNetCore.Mvc;

namespace SistemaCirculares.Controllers
{
    public class UserRegistrationController : Controller
    {
        public IActionResult UserRegistration()
        {
            return View();
        }
    }
}
