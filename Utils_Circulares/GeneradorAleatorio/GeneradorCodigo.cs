using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Utils_Circulares.GeneradorAleatorio
{
    public static class GeneradorCodigo
    {
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string GenerarCodigo()
        {
            Span<char> span = stackalloc char[8];

            span[0] = Letters[RandomNumberGenerator.GetInt32(26)];
            span[1] = Letters[RandomNumberGenerator.GetInt32(26)];
            span[2] = Letters[RandomNumberGenerator.GetInt32(26)];

            span[3] = (char)('0' + RandomNumberGenerator.GetInt32(10));
            span[4] = (char)('0' + RandomNumberGenerator.GetInt32(10));
            span[5] = (char)('0' + RandomNumberGenerator.GetInt32(10));
            span[6] = (char)('0' + RandomNumberGenerator.GetInt32(10));
            span[7] = (char)('0' + RandomNumberGenerator.GetInt32(10));

            return new string(span);
        }
    }
}
