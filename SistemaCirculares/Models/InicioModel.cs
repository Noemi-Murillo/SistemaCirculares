using Entities.Reply;
using Entities.UserLogin;
using System;


namespace SistemaCirculares.Models
{
    public class InicioModel : BaseAPI
    {

        private readonly IConfiguration _config;

        public InicioModel(IConfiguration config)
        {
            _config = config;
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
                var UrlAPI = _config["UrlAPICirculares"];
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
