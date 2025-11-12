using Entities_Circulares.Reply;
using DAL_Circulares;
using Entities_Circulares.UserRegistration;
using Utils_Circulares.GeneradorAleatorio;
using Utils_Circulares.Hashear;

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

                var VerificarHash = PasswordHash.Verify(ObjUsuario.Password, respuesta.Result.Password);


                if (respuesta != null && respuesta.Ok && VerificarHash)
                {
                    reply.Ok = true;
                    reply.Message = "Inicio de sesión exitoso.";
                    reply.Result = respuesta.Result;
                }
                else
                {
                    reply.Ok = false;
                    reply.Message = "Correo o contraseña incorrectos.";
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
