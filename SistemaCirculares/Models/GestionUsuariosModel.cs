using Entities_Circulares.Comites;
using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;
using System;
using System.Runtime.Intrinsics.Arm;
namespace SistemaCirculares.Models
{
    public class GestionUsuariosModel : BaseAPI
    {

        public GestionUsuariosModel(IConfiguration config) : base(config)
        {

        }

        public Reply<List<Usuario>> ObtenerUsuariosGestionComite()
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();
            UserRegistration ObjUsuario = new UserRegistration
            {
                Email = "",
                Password = ""
            };
            try
            {
                var UrlAPI = string.Format("{0}/GestionUsuarios/ObtenerUsuariosGestionComite", UrlApiCirculares);
                reply = PostAPI<Reply<List<Usuario>>, UserRegistration>(UrlAPI, ObjUsuario);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<string> GenerarCodigoRegistro(Usuario ObjUsuario)
        {

            Reply<string> reply = new Reply<string>();


            try
            {
                var UrlAPI = string.Format("{0}/GestionUsuarios/GenerarCodigoRegistro", UrlApiCirculares);
                reply = PostAPI<Reply<string>, Usuario>(UrlAPI, ObjUsuario);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<List<Comites>> ObtenerComites(int Parametro)
        {

            Reply<List<Comites>> reply = new Reply<List<Comites>>();

            try
            {
                var UrlAPI = string.Format("{0}/GestionUsuarios/ObtenerComites", UrlApiCirculares);
                reply = PostAPI<Reply<List<Comites>>, int>(UrlAPI, Parametro);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<List<Usuario>> GuardarUsuarios(List<Usuario> ObjUsuario)
        {

            Reply<List<Usuario>> reply = new Reply<List<Usuario>>();
            
            try
            {
                var UrlAPI = string.Format("{0}/GestionUsuarios/GuardarUsuario", UrlApiCirculares);
                reply = PostAPI<Reply<List<Usuario>>, List<Usuario>>(UrlAPI, ObjUsuario);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }

        public Reply<string> CrearUsuario(Usuario ObjUsuario)
        {

            Reply<string> reply = new Reply<string>();


            try
            {
                var UrlAPI = string.Format("{0}/GestionUsuarios/CrearUsuario", UrlApiCirculares);
                reply = PostAPI<Reply<string>, Usuario>(UrlAPI, ObjUsuario);



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = "Error, " + ex;
            }

            return reply;



        }
    }
}
