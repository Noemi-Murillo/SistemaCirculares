using Entities.Reply;
using Entities.UserLogin;
using System;
using System.Runtime.Intrinsics.Arm;


namespace SistemaCirculares.Models
{
    public class InicioModel : BaseAPI
    {
        public InicioModel(IConfiguration config) : base(config)
        {

        }

        public Reply<UserRegistration> LogIn(string Email, string Password)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();
            UserRegistration ObjUsuario = new UserRegistration
            {
                Email = Email,
                Password = Password
            };
            try
            {
                var UrlAPI = string.Format("{0}/LogIn", UrlApiCirculares);
                reply.Result = PostAPI<UserRegistration, UserRegistration>(UrlAPI, ObjUsuario);



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
