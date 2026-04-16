using BLL_Circulares;
using Entities_Circulares.Eventos;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Resources;
using Utils_Circulares.Correos;
using Utils_Circulares.PlantillaCorreo;

namespace API_Circulares.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircularController : ControllerBase
    {

        private readonly PublicarCircularBLL _AccesoCircularBLL;
        private readonly CircularesBLL _AccesoCirculaesBLL;
        private readonly IConfiguration _configuration;
        private readonly EmailService _Email;
        public CircularController(IConfiguration configuration, PublicarCircularBLL AccesoCircularBLL, CircularesBLL accesoCirculaesBLL, EmailService email)
        {

            _configuration = configuration;
            _AccesoCircularBLL = AccesoCircularBLL;
            _AccesoCirculaesBLL = accesoCirculaesBLL;
            _Email = email;
        }


        [HttpPost("PublicarCircular")]
        public async Task<Reply<bool>> PublicarCircular([FromBody] Circulares ObjCirculares)
        {
            Reply<bool> reply = new Reply<bool>();

            try
            {
                var respuesta = _AccesoCircularBLL.PublicarCircular(
                    ObjCirculares,
                    _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;

                    if (respuesta.Ok)
                    {
                        var respuestacorreos = _AccesoCirculaesBLL.ObtenerTodosCorreos(
                            _configuration.GetConnectionString("Conexion"));

                        List<string?> correos = respuestacorreos.Result
                            .Where(u => !string.IsNullOrWhiteSpace(u.Correo))
                            .Select(u => u.Correo)
                            .Distinct()
                            .ToList();

                        string html = Plantilla.Plantilla1
                            .Replace("{{TITULO_CIRCULAR}}", "Nueva Circular Publicada")
                            .Replace("{{RESUMEN}}", "Se ha realizado la publicación de una nueva circular, favor ingresar al sistema de gestión de circulares")
                            .Replace("{{FECHA_PUBLICACION}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                            .Replace("{{URL_SISTEMA}}", "http://localhost:5032/");

                        foreach (var correo in correos)
                        {
                            try
                            {
                                await _Email.SendAsync(
                                    "Aviso general",
                                    html,
                                    true,
                                    new[] { correo! },
                                    ObjCirculares.Archivo.ArchivoBytes,
                                    ObjCirculares.Archivo?.FileName ?? "Circular.pdf"
                                );
                            }
                            catch (Exception exCorreo)
                            {
                                Console.WriteLine($"Error enviando a {correo}: {exCorreo.Message}");
                            }
                        }
                    }
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
        [HttpPost("ObtenerCircularesPorCantidad")]
        public Reply<List<Circulares>> ObtenerCircularesPorCantidad([FromBody] Circulares ObjCirculares)
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();

            try
            {

                var respuesta = _AccesoCirculaesBLL.ObtenerCircularesPorCantidad(_configuration.GetConnectionString("Conexion"), ObjCirculares.IdComite);

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


        [HttpPost("ObtenerEventosCalendario")]
        public Reply<List<Eventos>> ObtenerEventosCalendario([FromBody] Eventos ObjCirculares)
        {

            Reply<List<Eventos>> reply = new Reply<List<Eventos>>();

            try
            {

                var respuesta = _AccesoCirculaesBLL.ObtenerEventosCalendario(_configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerEventosCalendario en la capa API {ex.Message}";
            }


            return reply;


        }


        [HttpPost("ObtenerCircularesPorId")]
        public Reply<Circulares> ObtenerCircularesPorId([FromBody] int IdCircular)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {

                var respuesta = _AccesoCirculaesBLL.ObtenerCircularPorId(IdCircular, _configuration.GetConnectionString("Conexion"));

                if (respuesta != null)
                {
                    reply = respuesta;
                }

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerCircularesPorId en la capa API {ex.Message}";
            }


            return reply;


        }


    }
}
