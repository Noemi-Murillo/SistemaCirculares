using BLL_Circulares;
using Entities_Circulares.FileCirculares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Entities_Circulares.Reply;

namespace API_Circulares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircularController : ControllerBase
    {

        private readonly PublicarCircularBLL _AccesoCircularBLL;
        private readonly CircularesBLL _AccesoCirculaesBLL;
        private readonly IConfiguration _configuration;
        public CircularController(IConfiguration configuration, PublicarCircularBLL AccesoCircularBLL, CircularesBLL accesoCirculaesBLL)
        {

            _configuration = configuration;
            _AccesoCircularBLL = AccesoCircularBLL;
            _AccesoCirculaesBLL = accesoCirculaesBLL;
        }


        [HttpPost("PublicarCircular")]
        public Reply<bool> PublicarCircular([FromBody] Circulares ObjCirculares)
        {

            Reply<bool> reply = new Reply<bool>();

            try
            {

                var respuesta = _AccesoCircularBLL.PublicarCircular(ObjCirculares, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método PublicarCircular en la capa API {ex.Message}";
            }


            return reply;


        }

        [HttpPost("ObtenerCirculares")]
        public Reply<List<Circulares>> ObtenerCirculares([FromBody] Circulares ObjCirculares)
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();

            try
            {

                var respuesta = _AccesoCirculaesBLL.ObtenerCirculares(_configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerCirculares en la capa API {ex.Message}";
            }


            return reply;


        }


    }
}
