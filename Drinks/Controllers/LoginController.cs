using Drinks.Models;
using Drinks.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Drinks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService service)
        {
            _userService = service;
        }

        [Route("token")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] UserRequest mainRequest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserResponse response = await _userService.Auth(mainRequest);
                    return response != null ? 
                                    new ObjectResult(response)
                                    {
                                        StatusCode = (int)HttpStatusCode.OK
                                    }
                                    : new ObjectResult(new { Message = "Get token failed, user not authorized" })
                                    {
                                        StatusCode = (int)HttpStatusCode.Unauthorized
                                    };
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new { Message = e.InnerException.Message ?? e.Message })
                {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                };
            }
            return new ObjectResult(null)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }
    }
}
