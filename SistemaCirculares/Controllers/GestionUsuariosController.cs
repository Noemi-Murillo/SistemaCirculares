using Entities_Circulares.Reply;
using Entities_Circulares.GestionUsuarios;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;

namespace SistemaCirculares.Controllers
{
    public class GestionUsuariosController : Controller
    {
        private readonly GestionUsuariosModel _AccesoGestionModel;

        // <<< Constructor con DI >>>
        public GestionUsuariosController(GestionUsuariosModel gestionUsuariosModel)
        {
            _AccesoGestionModel = gestionUsuariosModel;
        }

        public IActionResult Gestion()
        {
            ObtenerUsuariosGestionComite();
            return View();
        }


        
        public Reply<List<Usuario>> ObtenerUsuariosGestionComite()
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();

            try
            {
                var respuesta = _AccesoGestionModel.ObtenerUsuariosGestionComite();

                if (respuesta != null)
                {

                    reply = respuesta;
                    ViewBag.Usuarios = reply.Result;

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
