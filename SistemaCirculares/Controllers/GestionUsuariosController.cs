using Entities_Circulares.Reply;
using Entities_Circulares.GestionUsuarios;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;
using Entities_Circulares.Comites;

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
            ObtenerCookie();
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

        public Reply<string> GenerarCodigoRegistro([FromBody] Usuario ObjUsuario)
        {

            Reply<string> reply = new Reply<string>();

            try
            {
                var respuesta = _AccesoGestionModel.GenerarCodigoRegistro(ObjUsuario);

                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador GestionUsuarios método GenerarCodigoRegistro , {ex.Message}";
            }

            return reply;



        }

        public Reply<List<Comites>> ObtenerComites([FromBody] int Parametro)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {
                var respuesta = _AccesoGestionModel.ObtenerComites(Parametro);

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



        public Reply<List<Usuario>> GuardarUsuario([FromBody] List<Usuario> ObjUsuario)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();

            try
            {
                var respuesta = _AccesoGestionModel.GuardarUsuarios(ObjUsuario);

                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador GestionUsuarios método GuardarUsuario , {ex.Message}";
            }

            return reply;



        }

        public Reply<string> CrearUsuario([FromBody] Usuario ObjUsuario)
        {

            Reply<string> reply = new Reply<string>();

            try
            {
                var respuesta = _AccesoGestionModel.CrearUsuario(ObjUsuario);

                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador GestionUsuarios método CrearUsuario , {ex.Message}";
            }

            return reply;



        }

        public void ObtenerCookie()
        {

            try
            {

                string ValorCookieNombre = Request.Cookies["nombreCompleto"];

                if (ValorCookieNombre != null)
                {

                    ViewBag.NombreUsuario = ValorCookieNombre;

                }

                string ValorCookieComite = Request.Cookies["comite"];

                if (ValorCookieComite != null)
                {

                    ViewBag.Comite = ValorCookieComite;

                }


                string ValorCookieRol = Request.Cookies["rol"];

                if (ValorCookieRol != null)
                {

                    ViewBag.Rol = ValorCookieRol;

                }


            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


    }
}
