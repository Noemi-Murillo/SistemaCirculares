using Microsoft.AspNetCore.Mvc;
using Entities_Circulares.Reply;
using Entities_Circulares.FileCirculares;
using SistemaCirculares.Models;

namespace SistemaCirculares.Controllers
{
    public class SubirCircularesController : Controller
    {

        private readonly CircularesModel _AccesoCircularesModel;

        public SubirCircularesController(CircularesModel CircularesModel)
        {
            _AccesoCircularesModel = CircularesModel;
        }

        public IActionResult Publicarcircular()
        {
            ObtenerCookie();
            return View();
        }

        public IActionResult AlmacenamientoCirculares()
        {
            ObtenerCookie();

            return View();
        }

        [HttpPost]
        public Reply<bool> EnviarCircular([FromForm] string NombreCircular,
                                          [FromForm] bool SoloMiembros,
                                          [FromForm] string FechaEvento,
                                          [FromForm] string NombreEvento,
                                          [FromForm] IFormFile Archivo)
        {

            Reply<bool> reply = new Reply<bool>();

            try
            {

                FileCircular fileDto = null;
                if (Archivo != null && Archivo.Length > 0)
                {
                    using var ms = new MemoryStream();
                    Archivo.CopyTo(ms);
                    var bytes = ms.ToArray();

                    fileDto = new FileCircular
                    {
                        FileName = Archivo.FileName,
                        ContentType = Archivo.ContentType,
                        Length = Archivo.Length,
                        Base64 = Convert.ToBase64String(bytes)
                    };
                }

                // Construir el objeto para el API destino (JSON)
                var payload = new Circulares
                {
                    NombreCircular = NombreCircular,
                    SoloMiembros = SoloMiembros,
                    FechaEvento = FechaEvento, // idealmente en ISO 8601: yyyy-MM-dd o yyyy-MM-ddTHH:mm:ss
                    NombreEvento = NombreEvento,
                    Archivo = fileDto,
                    IdComite = Convert.ToInt32(Request.Cookies["comite"]),
                    IdUsuario = Convert.ToInt32(Request.Cookies["idUsuario"])
                };

                var respuesta = _AccesoCircularesModel.PublicarCircular(payload);

                if (respuesta != null)
                {

                    reply = respuesta;

                }






            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Result = false;
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
