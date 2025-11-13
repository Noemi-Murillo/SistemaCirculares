using DAL_Circulares;
using Entities_Circulares.FileCirculares;
using Entities_Circulares.Reply;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Circulares
{
    public class PublicarCircularBLL
    {

        private readonly PublicarCircularDAL _AccesoCircularDAL;


        public PublicarCircularBLL(PublicarCircularDAL CircularDAL)
        {

            _AccesoCircularDAL = CircularDAL;
        }


        public Reply<bool> PublicarCircular(Circulares ObjCircular, string Conexion)
        {

            Reply<bool> reply = new Reply<bool>();

            try
            {

                if (!string.IsNullOrWhiteSpace(ObjCircular.Archivo.Base64))
                {
                    var decoded = DecodeBase64DataUrl(ObjCircular.Archivo.Base64,
                                                      out string contentType,
                                                      out string fileName);

                    ObjCircular.Archivo.ArchivoBytes = decoded;

                    // Rellena metadatos si no venían
                    if (string.IsNullOrWhiteSpace(ObjCircular.Archivo.ContentType) && !string.IsNullOrWhiteSpace(contentType))
                        ObjCircular.Archivo.ContentType = contentType;

                    if (string.IsNullOrWhiteSpace(ObjCircular.Archivo.FileName) && !string.IsNullOrWhiteSpace(fileName))
                        ObjCircular.NombreCircular = fileName;

                    
                }

                var respuesta = _AccesoCircularDAL.PublicarCircular(ObjCircular, Conexion);

                if (respuesta != null)
                {
                    reply = respuesta;
                }



            }
            catch (Exception ex)
            {

                reply.Ok = false;
                reply.Message = $"Ha ocurrido un error en el método PublicarCircular en la capa BLL {ex.Message}";
            }

            return reply;

        }




        private static byte[] DecodeBase64DataUrl(string base64OrDataUrl, out string contentType, out string fileName)
        {
            contentType = null;
            fileName = null;
            var s = base64OrDataUrl.Trim();

            // Si viene como data URL: data:application/pdf;name=archivo.pdf;base64,AAA...
            int comma = s.IndexOf(',');
            if (comma > 0 && s[..comma].Contains(";base64", StringComparison.OrdinalIgnoreCase))
            {
                // Encabezado: data:...;...;base64
                var header = s[..comma];

                // content-type
                int colon = header.IndexOf(':');
                if (colon >= 0)
                {
                    var meta = header[(colon + 1)..]; // application/pdf;name=...;base64
                                                      // partir por ';'
                    foreach (var part in meta.Split(';'))
                    {
                        var p = part.Trim();
                        if (p.Equals("base64", StringComparison.OrdinalIgnoreCase)) continue;

                        if (p.Contains("/")) contentType = p; // ej: application/pdf

                        // nombre (no estándar, pero algunos clientes lo envían)
                        if (p.StartsWith("name=", StringComparison.OrdinalIgnoreCase))
                        {
                            var nv = p.Split('=', 2);
                            if (nv.Length == 2) fileName = nv[1].Trim().Trim('"');
                        }
                    }
                }

                s = s[(comma + 1)..]; // Dejar solo el payload base64
            }

            // Quitar espacios/saltos
            s = s.Replace("\r", "").Replace("\n", "").Trim();

            // Arreglar padding si falta
            int mod = s.Length % 4;
            if (mod != 0) s = s.PadRight(s.Length + (4 - mod), '=');

            return Convert.FromBase64String(s);
        }



    }
}
