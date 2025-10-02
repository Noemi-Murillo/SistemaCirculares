using Microsoft.AspNetCore.Mvc;
using Entities.Reply;
using Entities.UserLogin;

namespace SistemaCirculares.Controllers
{
    public class InicioController : Controller
    {
        public IActionResult Iniciosesion()
        {
            return View();
        }

        [HttpPost]
        public Reply<UserRegistration> LogIn(string Email, string Password)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {
                if (Email != "" && Email != null && Password != "" && Password != null)
                {

                    reply.Ok = true;
                    reply.Message = "Ha iniciado sesión de manera correcta";


                }
                else
                {
                    reply.Ok = false;
                    reply.Message = "Error, el usuario o la contraseña no es correcto";



                }


            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }



    }
}
