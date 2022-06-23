using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks.Services
{
    public interface IEncryptService
    {
        public string Encrypt(string plainText);
        public string Decrypt(string cypherText);
        public string EncryptUsingCertificate(string data);
        public string DecryptUsingCertificate(string data);
    }
}
