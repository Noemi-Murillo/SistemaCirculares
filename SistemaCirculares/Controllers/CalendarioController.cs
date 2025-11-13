using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace SistemaCirculares.Controllers
{
    public class CalendarioController : Controller
    {
        public IActionResult Calendario()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ObtenerEventos()
        {
            try
            {
                // Aquí obtienes los eventos de tu base de datos o servicio
                var eventos = new List<object>
                {
                    new
                    {
                        id = 1,
                        titulo = "Reunión de comité",
                        fechaInicio = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH:mm:ss"),
                        fechaFin = DateTime.Now.AddDays(1).AddHours(2).ToString("yyyy-MM-ddTHH:mm:ss"),
                        descripcion = "Reunión mensual del comité de calidad",
                        ubicacion = "Sala de juntas",
                        color = "#3788d8",
                        todoElDia = false,
                        idComite = 1,
                        nombreComite = "Comité de Calidad"
                    },
                    new
                    {
                        id = 2,
                        titulo = "Capacitación",
                        fechaInicio = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"),
                        fechaFin = DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"),
                        descripcion = "Capacitación en nuevas tecnologías",
                        ubicacion = "Auditorio principal",
                        color = "#28a745",
                        todoElDia = true,
                        idComite = 2,
                        nombreComite = "Comité de Tecnología"
                    }
                };

                return Json(new { ok = true, result = eventos, message = "Eventos obtenidos correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, result = new List<object>(), message = $"Error: {ex.Message}" });
            }
        }


    }
}
