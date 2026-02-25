using Microsoft.AspNetCore.Mvc;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;
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

                    if (respuesta.Ok)
                    {
                        var opcionesCookie = new CookieOptions
                        {
                            HttpOnly = true,               
                            Secure = true,                   
                            SameSite = SameSiteMode.Strict,  
                            Expires = DateTime.Now.AddHours(8) 
                        };

                        Response.Cookies.Append(
                            "nombreCompleto",
                            respuesta.Result.Nombre ?? string.Empty,
                            opcionesCookie
                        );

                        Response.Cookies.Append(
                            "rol",
                            respuesta.Result.IdRol.ToString() ?? string.Empty,
                            opcionesCookie
                        );

                        Response.Cookies.Append(
                            "comite",
                            respuesta.Result.IdComite.ToString() ?? string.Empty,
                            opcionesCookie
                        );

                        Response.Cookies.Append(
                         "idUsuario",
                         respuesta.Result.IdUsuario.ToString() ?? string.Empty,
                         opcionesCookie
                     );


                    }


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
