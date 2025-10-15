using Entities_Circulares.Reply;
using DAL_Circulares;
using Entities_Circulares.UserRegistration;
using Utils_Circulares.GeneradorAleatorio;

namespace BLL_Circulares
{
    public class InicioBLL
    {

        private readonly InicioDAL _AccesoInicioDal;

        public InicioBLL(InicioDAL inicioDAL)
        {

            _AccesoInicioDal = inicioDAL;
        }


        public Reply<UserRegistration> LogIn(UserRegistration ObjUsuario, string Conexion)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {

                var respuesta = _AccesoInicioDal.LogIn(ObjUsuario, Conexion);

                if (respuesta != null)
                {

                    reply = respuesta;

                }



            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método LogIn en la capa BLL {ex.Message}";

            }

            return reply;

        }






    }
}
