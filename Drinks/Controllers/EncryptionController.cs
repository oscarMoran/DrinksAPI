using Drinks.Models;
using Drinks.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Text.Json;

namespace Drinks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EncryptionController : Controller
    {
        private readonly IEncryptService _encriptor;
        public EncryptionController(IEncryptService encriptor)
        {
            _encriptor = encriptor;
        }

        [HttpPost]
        [Route("encript")]
        public async Task<IActionResult> EncriptionRsa([FromBody] MainRequest userInput)
        {
            string encripted;

            try
            {
                var data = JsonSerializer.Serialize(userInput);
                encripted = _encriptor.Encrypt(data);
                var response = new
                {
                    EncryptedValue = encripted
                };
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { ErrorMessage = "Service not available, try later." }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }

        [HttpPost]
        [Route("decriptdata")]
        public async Task<IActionResult> DecriptionData([FromHeader] string encriptedData)
        {
            string decripted;

            try
            {
                decripted = _encriptor.Decrypt(encriptedData);
                var data = JsonSerializer.Deserialize<MainRequest>(decripted);
                return new ObjectResult(data) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { ErrorMessage = "Service not available, try later." }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }

        [HttpPost]
        [Route("encriptwithcert")]
        public async Task<IActionResult> EncriptWithCertificate([FromBody] MainRequest userInput)
        {
            string encripted;

            try
            {
                var data = JsonSerializer.Serialize(userInput);
                encripted = _encriptor.EncryptUsingCertificate(data);
                var response = new
                {
                    EncryptedValue = encripted
                };
                return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { ErrorMessage = "Service not available, try later." }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }

        [HttpPost]
        [Route("decriptwithcert")]
        public async Task<IActionResult> DecriptWithCertificate([FromHeader] string encriptedData)
        {
            string decripted;

            try
            {
                decripted = _encriptor.DecryptUsingCertificate(encriptedData);
                var data = JsonSerializer.Deserialize<MainRequest>(decripted);
                return new ObjectResult(data) { StatusCode = (int)HttpStatusCode.OK };
            }
            catch (Exception e)
            {
                return new ObjectResult(new { ErrorMessage = "Service not available, try later." }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };
            }
        }
    }
}
