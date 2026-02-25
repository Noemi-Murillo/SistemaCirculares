using Entities_Circulares.Eventos;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Mvc;
using SistemaCirculares.Models;
using System;
using System.Collections.Generic;

namespace SistemaCirculares.Controllers
{
    public class CalendarioController : Controller
    {

        private readonly CircularesModel _AccesoCircularesModel;

        public CalendarioController(CircularesModel CircularesModel)
        {
            _AccesoCircularesModel = CircularesModel;
        }



        public IActionResult Calendario()
        {
            ObtenerCookie();
            return View();
        }

        [HttpPost]
        public IActionResult ObtenerEventos()
        {

            Reply<List<Eventos>> respuesta = new Reply<List<Eventos>>();

            try
            {


                var reply = _AccesoCircularesModel.ObtenerEventosCalendario();

                if (reply != null)
                {

                    respuesta = reply;


                }       

                return Json(new { ok = true, result = reply?.Result, message = "Eventos obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, result = new List<object>(), message = $"Error: {ex.Message}" });
            }
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
