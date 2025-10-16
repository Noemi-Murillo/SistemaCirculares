using DAL_Circulares;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Circulares
{
    public class GestionUsuariosBLL
    {

        private readonly GestionUsuariosDAL _AccesoGestionoDal;

        public GestionUsuariosBLL(GestionUsuariosDAL gestionUsuariosDAL)
        {

            _AccesoGestionoDal = gestionUsuariosDAL;
        }


        public Reply<List<Usuario>> ObtenerUsuariosGestionComite(string Conexion)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();

            try
            {

                var respuesta = _AccesoGestionoDal.ObtenerUsuariosGestionComite(Conexion);
                
                if (respuesta != null)
                {

                    reply = respuesta;

                }



            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerUsuariosGestionComite en la capa BLL {ex.Message}";

            }

            return reply;

        }



    }
}
