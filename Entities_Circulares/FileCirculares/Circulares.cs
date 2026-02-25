using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.FileCirculares
{
    public class Circulares
    {
        public int IdCircular { get; set; }
        public string? NombreCircular { get; set; }
        public bool SoloMiembros { get; set; }
        public string? FechaEvento { get; set; }
        public DateTime? FechaCircular { get; set; }
        public string? NombreEvento { get; set; }
        public FileCircular? Archivo { get; set; }
        public byte[]? ArchivoBytes { get; set; }
        public string? NombreComite { get; set; }
        public int IdComite { get; set; }
        public int IdUsuario { get; set; }


    }
}
