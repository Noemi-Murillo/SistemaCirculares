using BLL_Circulares;
using Entities_Circulares.Comites;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Circulares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GestionUsuariosController : ControllerBase
    {

        private readonly GestionUsuariosBLL _AccesoGestionBLL;
        private readonly IConfiguration _configuration;
        public GestionUsuariosController(IConfiguration configuration, GestionUsuariosBLL gestionUsuariosBLL)
        {

            _configuration = configuration;
            _AccesoGestionBLL = gestionUsuariosBLL;

        }

        [HttpPost("ObtenerUsuariosGestionComite")]
        public Reply<List<Usuario>> ObtenerUsuariosGestionComite([FromBody] Usuario ObjUsuario)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();

            try
            {

                var respuesta = _AccesoGestionBLL.ObtenerUsuariosGestionComite(_configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerUsuariosGestionComite en la capa API {ex.Message}";
            }


            return reply;


        }


        [HttpPost("GenerarCodigoRegistro")]
        public Reply<string> GenerarCodigoRegistro([FromBody] Usuario ObjUsuario)
        {

            Reply<string> reply = new Reply<string>();

            try
            {

                var respuesta = _AccesoGestionBLL.ObtenerCodigoRegistro(ObjUsuario, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerUsuariosGestionComite en la capa API {ex.Message}";
            }


            return reply;


        }

        [HttpPost("ObtenerComites")]
        public Reply<List<Comites>> ObtenerComites([FromBody] int Parametro)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {

                var respuesta = _AccesoGestionBLL.ObtenerComites(Parametro, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerUsuariosGestionComite en la capa API {ex.Message}";
            }


            return reply;


        }


        [HttpPost("GuardarUsuario")]
        public Reply<List<Usuario>> GuardarUsuario([FromBody] List<Usuario> ObjUsuario)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();

            try
            {

                var respuesta = _AccesoGestionBLL.GuardarUsuarios(ObjUsuario, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método GuardarUsuario en la capa API {ex.Message}";
            }


            return reply;


        }


    }
}
