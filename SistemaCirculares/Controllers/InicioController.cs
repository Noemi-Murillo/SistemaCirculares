using Microsoft.AspNetCore.Mvc;
using Entities.Reply;
using Entities.UserLogin;
using SistemaCirculares.Models;

namespace SistemaCirculares.Controllers
{
    public class InicioController : Controller
    {

        //Instancias del model

        private readonly InicioModel _AccesoInicioModel;

        // <<< Constructor con DI >>>
        public InicioController(InicioModel accesoInicioModel)
        {
            _AccesoInicioModel = accesoInicioModel;
        }
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
                var respuesta = _AccesoInicioModel.LogIn(Email, Password);

                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador InicioController método LogIn , {ex.Message}";
            }

            return reply;



        }



    }
}
