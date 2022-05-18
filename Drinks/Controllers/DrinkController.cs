using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sources.Tools.Factory;
using Sources.Tools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Drinks.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinkController : Controller
    {
        readonly IDbFactory _dbContext;
        public DrinkController(IDbFactory dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Route("getlist")]
        public async Task<IActionResult> Index()
        {
            var response = await _dbContext.GetInstance().GetDrinks();
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpGet]
        [Route("getDrink")]
        public async Task<IActionResult> GetDrink([FromHeader] string drinkId)
        {
            if (string.IsNullOrEmpty(drinkId))
            {
                return new ObjectResult(new { Message = "drink id value is missing" })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var response = await _dbContext.GetInstance().GetDrink(int.Parse(drinkId));
            return new ObjectResult(response) { StatusCode = (int)HttpStatusCode.OK };
        }

        [HttpPost]
        [Authorize]
        [Route("insert")]
        public async Task<IActionResult> InsertDrink([FromBody] Drink request)
        {
            Drink response = new Drink();
            try
            {
                if (ModelState.IsValid)
                {
                    response = await _dbContext.GetInstance().InsertDrink(request);
                    return new ObjectResult(response)
                    {
                        StatusCode = (int)HttpStatusCode.OK
                    };
                }
            }
            catch (Exception e)
            {
                return new ObjectResult(new { Message = e.Message ?? e.InnerException.Message }) {
                    StatusCode = (int)HttpStatusCode.ServiceUnavailable
                };
            }
            return new ObjectResult(response) { 
                StatusCode = (int)HttpStatusCode.BadRequest
            };
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public async Task<IActionResult> UpdateDrink([FromBody] Drink request)
        {
            var drink = await _dbContext.GetInstance().GetDrink(request.DrinkId);
            if (drink == null)
            {
                return new ObjectResult(new { Message = "It couldn't accomplished the transaction : threre's no information related." })
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
            var response = await _dbContext.GetInstance().UpdateDrink(request);
            if (response)
            {
                drink = await _dbContext.GetInstance().GetDrink(request.DrinkId);
                return new ObjectResult(drink) { StatusCode = (int)HttpStatusCode.OK };
            }
            return new ObjectResult(new { Message = "Try late, there's was an error" }) { StatusCode = (int)HttpStatusCode.ServiceUnavailable };
        }
    }
}
