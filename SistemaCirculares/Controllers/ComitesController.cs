using BLL_Circulares;
using Entities_Circulares.Comites;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;

namespace SistemaCirculares.Controllers
{
    public class ComitesController : Controller
    {

        //Instancias 


        private readonly ComitesModel _ComitesModel;
        private readonly IConfiguration _Config;
        //Constructor


        public ComitesController(ComitesModel comites, IConfiguration configuration)
        {

            _ComitesModel = comites;
            _Config = configuration;
        }


        public IActionResult GestionComites()
        {
            ObtenerUsuariosGestionComite();
            return View();
        }



        public Reply<List<Comites>> ObtenerUsuariosGestionComite()
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {
                var respuesta = _ComitesModel.ObtenerComites();

                if (respuesta != null)
                {

                    reply = respuesta;
                    ViewBag.ListaComites = reply.Result;

                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en la capa web en el controlador ComitesController método ObtenerUsuariosGestionComite , {ex.Message}";
            }

            return reply;



        }
    }
}
