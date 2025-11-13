using System.Diagnostics;
using Entities_Circulares.GestionUsuarios;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;
using Entities_Circulares.Reply;
using Entities_Circulares.FileCirculares;

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
    }
}
