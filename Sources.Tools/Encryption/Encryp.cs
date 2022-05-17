using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sources.Tools.Encryption
{
    public class Encryp
    {
        public static string GetSha256(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            SHA256 sha = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder sb = new StringBuilder();
            byte[] stream = sha.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++)
            {
                sb.AppendFormat("{0:x2}", stream[i]);
            }
            return sb.ToString();
        }
    }
}
