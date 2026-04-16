using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities_Circulares.FileCirculares
{
    public class FileCircular
    {
        public string? FileName { get; set; }
        public string? ContentType { get; set; }
        public long Length { get; set; }
        public string? Base64 { get; set; }
        public byte[]? ArchivoBytes { get; set; }
    }
}
