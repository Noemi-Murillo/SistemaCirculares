using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;
using BLL_Circulares;

namespace API_Circulares.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InicioController : ControllerBase
    {
        private readonly InicioBLL _AccesoInicioBLL;
        private readonly IConfiguration _configuration;
        public InicioController(IConfiguration configuration, InicioBLL inicioBLL)
        {

            _configuration = configuration;
            _AccesoInicioBLL = inicioBLL;

        }

        [HttpPost("LogIn")]
        public Reply<UserRegistration> LogIn([FromBody] UserRegistration ObjUsuario)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {

                var respuesta = _AccesoInicioBLL.LogIn(ObjUsuario, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método LogIn en la capa API {ex.Message}";
            }


            return reply;


        }


        [HttpPost("RestablecerContrasena")]
        public async Task<Reply<UserRegistration>> RestablecerContrasena([FromBody] UserRegistration ObjUsuario)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {

                var respuesta = await _AccesoInicioBLL.RestablecerContrasena(ObjUsuario, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método RestablecerContrasena en la capa API {ex.Message}";
            }


            return reply;


        }

    }
}
