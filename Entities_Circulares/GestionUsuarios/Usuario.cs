using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.GestionUsuarios
{
    public class Usuario
    {
        public int? Id { get; set; }
        public string? Nombre { get; set; }
        public int? IdComite { get; set; }
        public string? NombreComite { get; set; }
        public bool EsCordinador { get; set; }
        public bool Activo { get; set; }
        public string? CodigoRegistro { get; set; }
        public DateTime? FechaExpiracion { get; set; }
        public string? Apellido1 { get; set; }
        public string? Apellido2 { get; set; }
        public string? Correo { get; set; }
        public string? Contrasena { get; set; }
        public int? IdRol { get; set; }


    }
}
