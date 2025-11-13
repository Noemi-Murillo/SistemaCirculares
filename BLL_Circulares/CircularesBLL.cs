using DAL_Circulares;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Circulares
{
    public class CircularesBLL
    {

        private readonly CircularesDAL _AccesoCircularesDAL;


        public CircularesBLL(CircularesDAL CircularesDAL)
        {
            _AccesoCircularesDAL = CircularesDAL;
        }


        public Reply<List<Circulares>> ObtenerCirculares(string Conexion)
        {

            Reply<List<Circulares>> reply = new Reply<List<Circulares>>();

            try
            {

                var respuesta = _AccesoCircularesDAL.ObtenerCirculares(Conexion);
                if (respuesta != null)
                {
                    reply = respuesta;
                }




            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerCircularese en la capa BLL {ex.Message}";



            }

            return reply;
        }

        public Reply<Circulares> ObtenerCircularPorId(int IdCircular, string Conexion)
        {

            Reply<Circulares> reply = new Reply<Circulares>();

            try
            {

                var respuesta = _AccesoCircularesDAL.ObtenerCircularPorId(IdCircular, Conexion);
                if (respuesta != null)
                {
                    reply = respuesta;
                }




            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerCircularPorId en la capa BLL {ex.Message}";



            }

            return reply;
        }
    }
}
