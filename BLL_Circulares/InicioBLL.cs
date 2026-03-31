using DAL_Circulares;
using Entities_Circulares.Reply;
using Entities_Circulares.UserRegistration;
using Microsoft.Extensions.Configuration;
using Utils_Circulares.Correos;
using Utils_Circulares.GeneradorAleatorio;
using Utils_Circulares.Hashear;
using Utils_Circulares.PlantillaCorreo;
using System.Threading.Tasks;

namespace BLL_Circulares
{
    public class InicioBLL
    {

        private readonly InicioDAL _AccesoInicioDal;
        private readonly EmailService _Email;

        public InicioBLL(InicioDAL inicioDAL, EmailService emailService)
        {

            _AccesoInicioDal = inicioDAL;
            _Email = emailService;


        }


        public Reply<UserRegistration> LogIn(UserRegistration ObjUsuario, string Conexion)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {

                var respuesta = _AccesoInicioDal.LogIn(ObjUsuario, Conexion);

                var VerificarHash = false;
                if (respuesta.Ok)
                {

                    VerificarHash = PasswordHash.Verify(ObjUsuario.Password, respuesta.Result.Password);

                }



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



        public async Task<Reply<UserRegistration>> RestablecerContrasena(UserRegistration ObjUsuario, string Conexion)
        {

            Reply<UserRegistration> reply = new Reply<UserRegistration>();

            try
            {

                var Codigo = GeneradorCodigo.GenerarCodigo();

                string Password = "Pass" + Codigo;

                var contrasenaOculta = PasswordHash.Hash(Password);

                ObjUsuario.Password = contrasenaOculta;

                var respuesta = _AccesoInicioDal.RestablecerContrasena(ObjUsuario, Conexion);

                if (respuesta.Ok)
                {

                    reply = respuesta;

                    string html = Plantilla.PlantillaRecuperacionContrasena
                        .Replace("{{TITULO_CIRCULAR}}", "Restablecimiento de contraseña")
                        .Replace("{{RESUMEN}}", "Se ha generado una contraseña temporal para su acceso.")
                        .Replace("{{PASSWORD}}", Password ?? "")
                        .Replace("{{FECHA_PUBLICACION}}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                        .Replace("{{URL_SISTEMA}}", "http://localhost:5032/");


                    await _Email.SendAsync(
                         "Aviso general",
                         html,
                         true,
                         new[] { ObjUsuario.Email }

                     );

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
