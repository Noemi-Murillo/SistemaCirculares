using Entities_Circulares.GestionUsuarios;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;
using System;
using System.Runtime.Intrinsics.Arm;
namespace SistemaCirculares.Models
{
    public class GestionUsuariosModel: BaseAPI
    {

        public GestionUsuariosModel(IConfiguration config) : base(config)
        {

        }

        public  Reply<List<Usuario>>ObtenerUsuariosGestionComite()
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
    }
}
