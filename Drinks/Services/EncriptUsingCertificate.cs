using Drinks.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
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
        private readonly AppSettings _appsettings;
        private readonly string _publicPem;
        private readonly string _privatePem;
        public EncriptUsingCertificate(IWebHostEnvironment hostEnvironment, IOptions<AppSettings> appsettings)
        {
            _appsettings = appsettings.Value;
            _hostEnvironment = hostEnvironment;
            _publicPem = Path.Combine(_hostEnvironment.ContentRootPath, _appsettings.PubKeyName);
            _privatePem = Path.Combine(_hostEnvironment.ContentRootPath, _appsettings.PrivateKeyName);
        }

        public string Encrypt(string text)
        {
            try
            {
                using (TextReader publicKeyTextReader = new StringReader(File.ReadAllText(_publicPem)))
                {
                    RsaKeyParameters publicKeyParam = (RsaKeyParameters)new PemReader(publicKeyTextReader).ReadObject();
                    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKeyParam);
                    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(rsaParams);
                    var encryptedBytes = csp.Encrypt(Encoding.UTF8.GetBytes(text), false);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Decrypt(string encrypted)
        {
            try
            {
                using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(_privatePem)))
                {
                    AsymmetricCipherKeyPair readKeyPair = (AsymmetricCipherKeyPair)new PemReader(privateKeyTextReader).ReadObject();
                    RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)readKeyPair.Private);
                    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                    csp.ImportParameters(rsaParams);
                    var decryptedBytes = csp.Decrypt(Convert.FromBase64String(encrypted), false);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string DecryptUsingCertificate(string data)
        {
            throw new NotImplementedException();
        }

        public string EncryptUsingCertificate(string data)
        {
            throw new NotImplementedException();
        }
    }
}
