using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.EnvioCorreos
{
    public class eContenidoCorreo: eConfiguracionCorreo
    {
        public string? To { get; set; }
        public string? Body { get; set; }
        public string? Subject { get; set; }
        public string? Cc { get; set; }
        public string? Cco { get; set; }
        public int Institucion { get; set; }
        public string? NombreInstitucion { get; set; }
        public List<Attachment>? Adjuntos { get; set; }

    }
}
