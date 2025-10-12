using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.EnvioCorreos
{
    public class eConfiguracionCorreo
    {
        public string? SMTP_HOST { get; set; }
        public string? SMTP_PORT { get; set; }
        public string? SMTP_USER { get; set; }
        public string? SMTP_PASS { get; set; }
        public string? SMTP_DISPLAY_NAME { get; set; }
        public string? RutaLogoSinirube { get; set; }
    }
}
