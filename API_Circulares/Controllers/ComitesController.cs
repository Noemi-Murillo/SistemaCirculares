using BLL_Circulares;
using Entities_Circulares.Comites;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Circulares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComitesController : ControllerBase
    {


        //Instancias

        private readonly ComiteBLL _ComiteBLL;
        private readonly IConfiguration _Config;
        //Constructor 

        public ComitesController(ComiteBLL comite, IConfiguration configuration)
        {


            _ComiteBLL = comite;
            _Config = configuration;

        }


        [HttpPost("ObtenerComites")]
        public Reply<List<Comites>> ObtenerComites([FromBody] Comites ObjComite)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {

                var respuesta = _ComiteBLL.ObtenerComites(_Config.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerComites en la capa API {ex.Message}";
            }


            return reply;


        }






    }
}
