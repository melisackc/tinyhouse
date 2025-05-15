using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProject
{
    internal class Sifreleme 
    {
        public static string HashleSHA256(string sifre)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] sifreBytes = Encoding.UTF8.GetBytes(sifre);
                byte[] hashBytes = sha256.ComputeHash(sifreBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

    }
}
