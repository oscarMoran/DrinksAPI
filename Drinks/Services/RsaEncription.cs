
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Drinks.Services
{
    public class RsaEncription
    {
        private static IWebHostEnvironment _hostEnvironment;
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider(2048);
        private RSAParameters _privateKey;
        private RSAParameters _publicKey;

        public RsaEncription(IWebHostEnvironment hostEnvironment)
        {
            _publicKey = csp.ExportParameters(true);
            _privateKey = csp.ExportParameters(true);
            _hostEnvironment = hostEnvironment;
        }

        public string Encrypt(string plainText)
        {
            csp = new RSACryptoServiceProvider();
            csp.ImportParameters(_publicKey);
            var data = Encoding.Unicode.GetBytes(plainText);
            var cypher = csp.Encrypt(data, false);
            return Convert.ToBase64String(cypher);
        }

        public string Decrypt(string cypherText)
        {
            var dataBytes = Convert.FromBase64String(cypherText);
            csp.ImportParameters(_privateKey);
            var pkk = csp.ExportRSAPrivateKey();
            var plainText = csp.Decrypt(dataBytes, false);
            return Encoding.Unicode.GetString(plainText);
        }


        public string EncryptUsingCertificate(string data)
        {
            var output = "";
            try
            {
                byte[] byteData = Encoding.UTF8.GetBytes(data);
                string path = Path.Combine(_hostEnvironment.ContentRootPath, "mycert.pem");
                var collection = new X509Certificate2Collection();
                collection.Import(path);
                var certificate = collection[0];
                using (RSA csp = (RSA)certificate.PublicKey.Key)
                {
                    byte[] bytesEncrypted = csp.Encrypt(byteData, RSAEncryptionPadding.OaepSHA1);
                    output = Convert.ToBase64String(bytesEncrypted);
                }
                return output;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string DecryptUsingCertificate(string data)
        {
            try
            {
                byte[] byteData = Convert.FromBase64String(data);
                string path = Path.Combine(_hostEnvironment.ContentRootPath, "mycertprivatekey.pfx");
                var Password = "Alsea.2022";
                var collection = new X509Certificate2Collection();
                collection.Import(System.IO.File.ReadAllBytes(path), Password, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
                X509Certificate2 certificate = new X509Certificate2();
                certificate = collection[0];
                foreach (var cert in collection)
                {
                    if (cert.FriendlyName.Contains("StarbucksEncryiptCertificate"))
                    {
                        certificate = cert;
                    }
                }
                if (certificate.HasPrivateKey)
                {
                    RSA csp = (RSA)certificate.PrivateKey;
                    var privateKey = certificate.PrivateKey as RSACryptoServiceProvider;
                    var keys = Encoding.UTF8.GetString(csp.Decrypt(byteData, RSAEncryptionPadding.OaepSHA1));
                    return keys;
                }
            }
            catch (Exception ex) { 
                return $"{ex}";
            }
            return null;
        }
    }
}
