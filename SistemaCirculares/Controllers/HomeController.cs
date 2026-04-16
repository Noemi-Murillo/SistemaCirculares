using Entities_Circulares.FileCirculares;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;
using System.Diagnostics;
using System.IO.Compression;

namespace SistemaCirculares.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CircularesModel _circularesModel;
        public HomeController(ILogger<HomeController> logger, CircularesModel circularesModel)
        {
            _logger = logger;
            _circularesModel = circularesModel;
        }

        public IActionResult Index()
        {
            ObtenerCookie();
            ObtenerCirculares();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public Reply<List<Circulares>> ObtenerCirculares()
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();

            try
            {
                var respuesta = _circularesModel.ObtenerCirculares();

                if (respuesta != null)
                {

                    reply = respuesta;
                    ViewBag.Circulares = reply.Result;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador HomeController método ObtenerUsuariosGestionComite , {ex.Message}";
            }

            return reply;



        }
        
        [HttpPost]
        public IActionResult ObtenerCircularesPorCantidad([FromBody] Circulares ObjCircular)
        {
            try
            {
                var respuesta = _circularesModel.ObtenerCircularesPorCantidad(ObjCircular.IdComite);

                if (respuesta != null && respuesta.Result != null)
                {
                    return DescargarCircularesZip(respuesta.Result);
                }

                return BadRequest("No hay circulares");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public IActionResult DescargarCircularesZip(List<Circulares> listaCirculares)
        {
            using (var memoria = new MemoryStream())
            {
                using (var zip = new ZipArchive(memoria, ZipArchiveMode.Create, true))
                {
                    foreach (var circular in listaCirculares)
                    {
                        if (circular.ArchivoBytes != null)
                        {
                            string nombreArchivo = circular.NombreCircular;

                            // asegurarse que tenga extensión
                            if (!nombreArchivo.EndsWith(".pdf"))
                                nombreArchivo += ".pdf";

                            var entrada = zip.CreateEntry(nombreArchivo);

                            using (var streamEntrada = entrada.Open())
                            {
                                streamEntrada.Write(circular.ArchivoBytes, 0, circular.ArchivoBytes.Length);
                            }
                        }
                    }
                }

                memoria.Position = 0;

                return File(memoria.ToArray(),
                            "application/zip",
                            "Circulares.zip");
            }
        }

        public Reply<Circulares> ObtenerCircularesPorId([FromBody] int IdCircular)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {
                var respuesta = _circularesModel.ObtenerCircularPorId(IdCircular);

         

                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador HomeController método ObtenerCircularesPorId , {ex.Message}";
            }

            return reply;



        }

        public Reply<Circulares> DescargarCircularesPorId([FromBody] int IdCircular)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {
                var respuesta = _circularesModel.ObtenerCircularPorId(IdCircular);



                if (respuesta != null)
                {

                    reply = respuesta;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador HomeController método ObtenerCircularesPorId , {ex.Message}";
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
