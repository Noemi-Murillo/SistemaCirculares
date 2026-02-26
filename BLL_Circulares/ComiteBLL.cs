using DAL_Circulares;
using Entities_Circulares.Comites;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Circulares
{
    public class ComiteBLL
    {


        //Instancias


        private readonly ComitesDAL _ComiteDAL;

        //Construtor
        public ComiteBLL(ComitesDAL comitesDAL)
        {


            _ComiteDAL = comitesDAL;


        }



        public Reply<List<Comites>> ObtenerComites(string Conexion)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {

                var respuesta = _ComiteDAL.ObtenerComites(Conexion);
                if (respuesta != null)
                {
                    reply = respuesta;
                }




            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerComites en la capa BLL {ex.Message}";



            }

            return reply;
        }


        public Reply<bool> CrearComite(Comites ObjComite, string Conexion)
        {

            Reply<bool> reply = new Reply<bool>();

            try
            {

                var respuesta = _ComiteDAL.CrearComite(ObjComite, Conexion);
                if (respuesta != null)
                {
                    reply = respuesta;
                }




            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerComites en la capa BLL {ex.Message}";



            }

            return reply;
        }

        public Reply<bool> EliminarComite(Comites ObjComite, string Conexion)
        {

            Reply<bool> reply = new Reply<bool>();

            try
            {

                var respuesta = _ComiteDAL.EliminarComite(ObjComite, Conexion);
                if (respuesta != null)
                {
                    reply = respuesta;
                }




            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerComites en la capa BLL {ex.Message}";



            }

            return reply;
        }






    }
}
