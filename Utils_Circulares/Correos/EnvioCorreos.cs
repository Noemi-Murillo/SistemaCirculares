using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text;
using Entities_Circulares.Reply;
using Entities_Circulares.EnvioCorreos;
using Microsoft.Extensions.Configuration;

namespace Utils_Circulares.Correos
{
    public class EnvioCorreos
    {
        #region ConstructorHerramientas

        /// <summary>
        /// Constructor de dependencias
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly eContenidoCorreo? _contenidoCorreo;

        public EnvioCorreos(IConfiguration configuration)
        {
            _configuration = configuration;
            _contenidoCorreo = _configuration.GetSection("AjustesServicioCorreo").Get<eContenidoCorreo>();
        }

        #endregion

        private const string _Destinatarios = "TipoContactoNotificacionCargaDestinatario";
        private const string _Copia = "TipoContactoNotificacionCargaCopia";
        private const string _CopiaOculta = "TipoContactoNotificacionCargaCopiaOculta";
        private const string _CorreoSinirubeError = "renlopez09@gmail.com";


        /// <summary>
        /// Este método se encarga de realizar toda la configuración previa al envío de correos
        /// </summary>
        /// <param name="ContenidoEnvioCorreos">Recibe un parámetro de tipo ContenidoCorreo</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Reply<bool> EnviarCorreo(eContenidoCorreo ContenidoEnvioCorreos)
        {
            Reply<bool> reply = new Reply<bool>();

            var ConfiguracionesEnvioCorreos = ObtenerConfiguracionesEnvioCorreo();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                MailMessage Mail = new MailMessage
                {
                    Body = ContenidoEnvioCorreos.Body,
                    IsBodyHtml = true,
                    From = new MailAddress(ConfiguracionesEnvioCorreos.Result?.SMTP_USER, ConfiguracionesEnvioCorreos.Result?.SMTP_DISPLAY_NAME, Encoding.UTF8),
                    Subject = ContenidoEnvioCorreos.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    Priority = MailPriority.Normal
                };

                if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.To))
                {

                    // Procesar destinatarios "To"
                    foreach (string email in ContenidoEnvioCorreos.To.Split(';'))
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            Mail.To.Add(new MailAddress(email.Trim()));
                        }
                    }

                    // Procesar destinatarios "CC"
                    if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.Cc))
                    {
                        foreach (string email in ContenidoEnvioCorreos.Cc.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                Mail.CC.Add(new MailAddress(email.Trim()));
                            }
                        }
                    }

                    // Procesar destinatarios "BCC"
                    if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.Cco))
                    {
                        foreach (string email in ContenidoEnvioCorreos.Cco.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                Mail.Bcc.Add(new MailAddress(email.Trim()));
                            }
                        }
                    }

                    // Procesar adjuntos
                    if (ContenidoEnvioCorreos.Adjuntos != null && ContenidoEnvioCorreos.Adjuntos.Any())
                    {
                        foreach (Attachment adjunto in ContenidoEnvioCorreos.Adjuntos)
                        {
                            Mail.Attachments.Add(adjunto);
                        }
                    }

                    // Añadir logo si es necesario
                    if (Mail.Body.Contains("[LOGO]"))
                    {
                        Mail = AddLogo(Mail, ConfiguracionesEnvioCorreos);
                    }

                    // Configurar y enviar el correo
                    SmtpClient SMTP = new SmtpClient
                    {
                        Credentials = new System.Net.NetworkCredential(ConfiguracionesEnvioCorreos.Result.SMTP_USER, ConfiguracionesEnvioCorreos.Result.SMTP_PASS),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Host = ConfiguracionesEnvioCorreos.Result.SMTP_HOST,
                        Port = Convert.ToInt32(ConfiguracionesEnvioCorreos.Result.SMTP_PORT),
                        EnableSsl = true
                    };


                    SMTP.Send(Mail);

                    reply.Ok = true;
                    reply.Message = "Los correos fueron enviados de manera exitosa";
                }
                else
                {
                    reply.Ok = true;
                    reply.Message = $"La institución {ContenidoEnvioCorreos.NombreInstitucion} no presenta correos disponibles para enviar los informes";
                    var ContenidoEnviar = ObtenerConfiguracionesDestinatarioCasoError(/*ArchivoRecursos.RecursosCorreo.PlantillaCorreoError*/"Error: ", "Sistema Circulares, ", $"los correos, la institución {ContenidoEnvioCorreos.NombreInstitucion} no presenta correos disponibles para enviar los informes");
                    EnviarCorreoCasoError(ContenidoEnviar.Result);
                }
            }
            catch (SmtpException ex)
            {
                // Manejar excepciones relacionadas con SMTP
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                reply.Ok = false;
                reply.Message = $"Ocurrió un error al enviar los correos: {ex.Message}";

            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(1024);
                sb.Append("\nTo:" + ContenidoEnvioCorreos.To);
                sb.Append("\nbody:" + ContenidoEnvioCorreos.Body);
                sb.Append("\nsubject:" + ContenidoEnvioCorreos.Subject);
                sb.Append("\nfromAddress:" + ConfiguracionesEnvioCorreos.Result.SMTP_USER);
                sb.Append("\nfromDisplay:" + ContenidoEnvioCorreos.SMTP_DISPLAY_NAME);
                sb.Append("\ncredentialUser:" + ConfiguracionesEnvioCorreos.Result.SMTP_USER);
                sb.Append("\ncredentialPasswordto:" + ConfiguracionesEnvioCorreos.Result.SMTP_PASS);
                sb.Append("\nHosting:" + ConfiguracionesEnvioCorreos.Result.SMTP_HOST);
                sb.Append("Error:" + ex.Message.ToString());
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                reply.Ok = false;
                reply.Message = $"Ocurrió un error al enviar los correos: {ex.Message}";

            }


            return reply;
        }

        /// <summary>
        /// Envia un correo en caso de error cuando una institución no posee correos electrónicos a enviar o cuando ocurre alguna excepción
        /// </summary>
        /// <param name="ContenidoEnvioCorreos">Cuerpo del correo</param>
        /// <returns>Retorna un objeto de tipo: Reply<eReporteDuplicidades></returns>
        /// <exception cref="Exception"></exception>
        public Reply<bool> EnviarCorreoCasoError(eContenidoCorreo ContenidoEnvioCorreos)
        {
            Reply<bool> reply = new Reply<bool>();

            var ConfiguracionesEnvioCorreos = ObtenerConfiguracionesEnvioCorreo();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


                MailMessage Mail = new MailMessage
                {
                    Body = ContenidoEnvioCorreos.Body,
                    IsBodyHtml = true,
                    From = new MailAddress(ConfiguracionesEnvioCorreos.Result?.SMTP_USER, ConfiguracionesEnvioCorreos.Result?.SMTP_DISPLAY_NAME, Encoding.UTF8),
                    Subject = ContenidoEnvioCorreos.Subject,
                    SubjectEncoding = Encoding.UTF8,
                    Priority = MailPriority.Normal
                };

                if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.To))
                {

                    // Procesar destinatarios "To"
                    foreach (string email in ContenidoEnvioCorreos.To.Split(';'))
                    {
                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            Mail.To.Add(new MailAddress(email.Trim()));
                        }
                    }

                    // Procesar destinatarios "CC"
                    if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.Cc))
                    {
                        foreach (string email in ContenidoEnvioCorreos.Cc.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                Mail.CC.Add(new MailAddress(email.Trim()));
                            }
                        }
                    }

                    // Procesar destinatarios "BCC"
                    if (!string.IsNullOrWhiteSpace(ContenidoEnvioCorreos.Cco))
                    {
                        foreach (string email in ContenidoEnvioCorreos.Cco.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(email))
                            {
                                Mail.Bcc.Add(new MailAddress(email.Trim()));
                            }
                        }
                    }

                    // Procesar adjuntos
                    if (ContenidoEnvioCorreos.Adjuntos != null && ContenidoEnvioCorreos.Adjuntos.Any())
                    {
                        foreach (Attachment adjunto in ContenidoEnvioCorreos.Adjuntos)
                        {
                            Mail.Attachments.Add(adjunto);
                        }
                    }

                    // Añadir logo si es necesario
                    if (Mail.Body.Contains("[LOGO]"))
                    {
                        Mail = AddLogo(Mail, ConfiguracionesEnvioCorreos);
                    }

                    // Configurar y enviar el correo
                    SmtpClient SMTP = new SmtpClient
                    {
                        Credentials = new System.Net.NetworkCredential(ConfiguracionesEnvioCorreos.Result.SMTP_USER, ConfiguracionesEnvioCorreos.Result.SMTP_PASS),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Host = ConfiguracionesEnvioCorreos.Result.SMTP_HOST,
                        Port = Convert.ToInt32(ConfiguracionesEnvioCorreos.Result.SMTP_PORT),
                        EnableSsl = true
                    };


                    SMTP.Send(Mail);

                    reply.Ok = true;
                    reply.Message = "Los correos fueron enviados de manera exitosa";
                }
                else
                {
                    reply.Ok = false;
                    reply.Message = "La institución no presenta correos disponibles para enviar los informes";
                }
            }
            catch (SmtpException ex)
            {
                // Manejar excepciones relacionadas con SMTP
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                reply.Ok = false;
                reply.Message = $"Ocurrió un error al enviar los correos: {ex.Message}";
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder(1024);
                sb.Append("\nTo:" + ContenidoEnvioCorreos.To);
                sb.Append("\nbody:" + ContenidoEnvioCorreos.Body);
                sb.Append("\nsubject:" + ContenidoEnvioCorreos.Subject);
                sb.Append("\nfromAddress:" + ConfiguracionesEnvioCorreos.Result.SMTP_USER);
                sb.Append("\nfromDisplay:" + ContenidoEnvioCorreos.SMTP_DISPLAY_NAME);
                sb.Append("\ncredentialUser:" + ConfiguracionesEnvioCorreos.Result.SMTP_USER);
                sb.Append("\ncredentialPasswordto:" + ConfiguracionesEnvioCorreos.Result.SMTP_PASS);
                sb.Append("\nHosting:" + ConfiguracionesEnvioCorreos.Result.SMTP_HOST);
                sb.Append("Error:" + ex.Message.ToString());

                throw new Exception(sb.ToString());

            }


            return reply;
        }



        /// <summary>
        /// Obtener y añadir logo al cuerpo del correo electrónico
        /// </summary>
        /// <param name="mail">parámetro Mail</param>
        /// <returns>Retorna el cuerpo de correo con el logo</returns>
        private static MailMessage AddLogo(MailMessage mail, Reply<eContenidoCorreo> ContenidoCorreo)
        {
            var vRutaLogo = Path.Combine(AppContext.BaseDirectory, ContenidoCorreo.Result.RutaLogoSinirube);
            var inline = new Attachment(vRutaLogo);
            inline.ContentId = Guid.NewGuid().ToString();
            inline.ContentDisposition.Inline = true;
            inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
            mail.Body = mail.Body.Replace("[LOGO]", string.Format(@"<img src='cid:{0}'/>", inline.ContentId));
            mail.Attachments.Add(inline);
            return mail;
        }


        #region Obtención de credenciales para envío de correo
        /// <summary>
        /// Método que obtiene las configuraciones básicas de envío de correos
        /// </summary>
        /// <returns></returns>
        private Reply<eContenidoCorreo> ObtenerConfiguracionesEnvioCorreo()
        {

            Reply<eContenidoCorreo> reply = new Reply<eContenidoCorreo>();

            eContenidoCorreo ContenidoEnvioCorreos = new eContenidoCorreo();

            try
            {
                ContenidoEnvioCorreos.SMTP_USER = _contenidoCorreo?.SMTP_USER;
                ContenidoEnvioCorreos.SMTP_PASS = _contenidoCorreo?.SMTP_PASS;
                ContenidoEnvioCorreos.SMTP_HOST = _contenidoCorreo?.SMTP_HOST;
                ContenidoEnvioCorreos.SMTP_PORT = _contenidoCorreo?.SMTP_PORT;
                ContenidoEnvioCorreos.SMTP_DISPLAY_NAME = _contenidoCorreo?.SMTP_DISPLAY_NAME;
                ContenidoEnvioCorreos.RutaLogoSinirube = _contenidoCorreo?.RutaLogoSinirube;

                reply.Ok = true;
                reply.Message = $"Configuraciones obtenidas de manera correcta";
                reply.Result = ContenidoEnvioCorreos;
            }
            catch (Exception ex)
            {
                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error al intentar obtener las configuraciones de envío de correos electrónicos: {ex.Message.ToString()}";

            }

            return reply;


        }






        #endregion


        #region Obtención de destinatarios, Copia, CopiaOculta

        /// <summary>
        /// Obtiene la configuración de destinatarios de las instituciones
        /// </summary>
        /// <param name="PlantillaCorreo"></param>
        /// <param name="RutaArchivo"></param>
        /// <param name="Siglas"></param>
        /// <param name="ListaInstitucionesBeneficios"></param>
        /// <returns>Retorna un objeto de tipo: Reply<eContenidoCorreo></returns>
        //public Reply<eContenidoCorreo> ObtenerConfiguracionesDestinatariosCorreo(string PlantillaCorreo, string RutaArchivo, string Siglas, eInstitucionBeneficioEnvioCorreo ListaInstitucionesBeneficios)
        //{

        //    Reply<eContenidoCorreo> reply = new Reply<eContenidoCorreo>();

        //    try
        //    {
        //        ListaInstitucionesBeneficios.ListaBeneficios = string.Join(",", ListaInstitucionesBeneficios.ListaIdBeneficios);
        //        List<Attachment> adjuntos = new List<Attachment>();

        //        var archivoAdjunto = new Attachment(RutaArchivo);
        //        adjuntos.Add(archivoAdjunto);

        //        var DestinatariosCorreo = _AccesoParametrizacionDAL.ObtenerDestinatariosCopiaCopiaOculta(ListaInstitucionesBeneficios.IdInstitucion, ListaInstitucionesBeneficios.ListaBeneficios, _Destinatarios);

        //        var CopiaCorreo = _AccesoParametrizacionDAL.ObtenerDestinatariosCopiaCopiaOculta(ListaInstitucionesBeneficios.IdInstitucion, ListaInstitucionesBeneficios.ListaBeneficios, _Copia);

        //        var CopiaOcultaCorreo = _AccesoParametrizacionDAL.ObtenerDestinatariosCopiaCopiaOculta(ListaInstitucionesBeneficios.IdInstitucion, ListaInstitucionesBeneficios.ListaBeneficios, _CopiaOculta);



        //        eContenidoCorreo Contenido = new eContenidoCorreo
        //        {
        //            To = DestinatariosCorreo.ValorRetorno?.To,
        //            Cc = CopiaCorreo.ValorRetorno?.To,
        //            Cco = CopiaOcultaCorreo.ValorRetorno?.To,
        //            Body = string.Format(PlantillaCorreo, Siglas, DateTime.Now.ToShortDateString()),
        //            Subject = "Notificación sobre Duplicidad de Beneficios RUB",
        //            Adjuntos = adjuntos,
        //            NombreInstitucion = Siglas


        //        };


        //        reply.Ok = true;
        //        reply.Msg = "Configuración de destinatarios realizada de manera correcta";
        //        reply.ValorRetorno = Contenido;

        //    }
        //    catch (Exception ex)
        //    {

        //        reply.Ok = false;
        //        reply.Msg = $"Ha ocurrido un error al obtener los destinatarios {ex.Message}";



        //    }


        //    return reply;

        //}

        /// <summary>
        /// Obtención de correo electrónico en caso de que suceda un error al procesar el archivo CSV
        /// </summary>
        /// <param name="PlantillaCorreo"></param>
        /// <param name="Siglas"></param>
        /// <param name="Metodo"></param>
        /// <returns>Retorna un objeto de tipo: Reply<eContenidoCorreo></returns>
        public Reply<eContenidoCorreo> ObtenerConfiguracionesDestinatarioCasoError(string PlantillaCorreo, string Siglas, string Metodo)
        {

            Reply<eContenidoCorreo> reply = new Reply<eContenidoCorreo>();

            try
            {

                eContenidoCorreo Contenido = new eContenidoCorreo
                {
                    To = _CorreoSinirubeError,
                    Body = string.Format(PlantillaCorreo, Siglas, DateTime.Now.ToShortDateString()),
                    Subject = string.Format("Error a la hora de obtener {0}", Metodo)
                };


                reply.Ok = true;
                reply.Message = "Configuración de destinatarios realizada de manera correcta";
                reply.Result = Contenido;

            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error al obtener los destinatarios {ex.Message}";



            }


            return reply;

        }
    }
    #endregion
}
