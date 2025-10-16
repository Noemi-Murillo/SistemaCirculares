using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.GestionUsuarios
{
    public class Usuario
    {
        public int Id {  get; set; }
        public string? Nombre { get; set; }
        public string? NombreComite { get; set; }
        public bool EsCordinador { get; set; }
        public bool Activo {  get; set; }


    }
}
