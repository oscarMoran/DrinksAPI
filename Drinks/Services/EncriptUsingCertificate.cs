using Microsoft.AspNetCore.Hosting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Drinks.Services
{
    public class EncriptUsingCertificate : IEncryptService
    {
        private static IWebHostEnvironment _hostEnvironment;
        private readonly RSACryptoServiceProvider _rsaPrivateProvider;
        private readonly RSACryptoServiceProvider _rsaPublicProvider;
        public EncriptUsingCertificate(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
            string public_pem = Path.Combine(_hostEnvironment.ContentRootPath, "publicKeySbx.pem");
            string private_pem = Path.Combine(_hostEnvironment.ContentRootPath, "privateKeySbx.pem");

            _rsaPublicProvider = GetPublicKeyFromPemFile(public_pem);
            _rsaPrivateProvider = GetPrivateKeyFromPemFile(private_pem);
        }

        public string Encrypt(string text)
        {
            var encryptedBytes = _rsaPublicProvider.Encrypt(Encoding.UTF8.GetBytes(text), false);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encrypted)
        {
            var decryptedBytes = _rsaPrivateProvider.Decrypt(Convert.FromBase64String(encrypted), false);
            return Encoding.UTF8.GetString(decryptedBytes);
        }

        public string DecryptUsingCertificate(string data)
        {
            throw new NotImplementedException();
        }

        public string EncryptUsingCertificate(string data)
        {
            throw new NotImplementedException();
        }


        private RSACryptoServiceProvider GetPrivateKeyFromPemFile(string filePath)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        private RSACryptoServiceProvider GetPublicKeyFromPemFile(string filePath)
        {
            using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(filePath)))
            {
                RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();

                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }
    }
}
