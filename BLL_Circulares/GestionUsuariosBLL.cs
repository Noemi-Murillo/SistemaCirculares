using DAL_Circulares;
using Entities_Circulares.Comites;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils_Circulares.GeneradorAleatorio;

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


        public Reply<string> ObtenerCodigoRegistro(Usuario ObjUsuario, string Conexion)
        {

            Reply<string> reply = new Reply<string>();

            try
            {
                var respuesta = GeneradorCodigo.GenerarCodigo();
                ObjUsuario.CodigoRegistro = respuesta;
                var respuestaNuevoCodigo = _AccesoGestionoDal.GenerarCodigoRegistro(ObjUsuario, Conexion);

                if (respuesta != null && respuestaNuevoCodigo != null && respuestaNuevoCodigo.Ok)
                {
                    reply.Ok = true;
                    reply.Message = respuestaNuevoCodigo.Message;
                    reply.Result = respuesta;

                }
                else
                {
                    reply.Ok = false;
                    reply.Message = respuestaNuevoCodigo.Message;
                }



            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método ObtenerCodigoRegistro en la capa BLL {ex.Message}";

            }

            return reply;

        }


        public Reply<List<Comites>> ObtenerComites(int Parametro, string Conexion)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {

                var respuesta = _AccesoGestionoDal.ObtenerComites(Parametro, Conexion);

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
