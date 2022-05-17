using Drinks.Models;
using Sources.Tools.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sources.Tools.Encryption;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Sources.Tools.Models;

namespace Drinks.Services
{
    public class UserService : IUserService
    {
        readonly IDbFactory _dbContext;
        private readonly AppSettings _appsettings;
        public UserService(IDbFactory dbContext, IOptions<AppSettings> appsettings)
        {
            _dbContext = dbContext;
            _appsettings = appsettings.Value;
        }

        public async Task<UserResponse> Auth(UserRequest model)
        {
            UserResponse response = null;
            try
            {
                var passwordHashed = Encryp.GetSha256(model.Password);
                var userRespDb = await _dbContext.GetInstance().GetUserByUserName(model.UserName);
                if (passwordHashed == userRespDb.Password && model.UserName == userRespDb.UserName)
                {
                    response = new UserResponse
                    {
                        Email = userRespDb.Email,
                        Message = "Authorized",
                        Token = GenerateToken(userRespDb)
                    };
                }
            }
            catch (Exception)
            {
                return null;
            }
            return response;
        }

        private string GenerateToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appsettings.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                            new Claim(ClaimTypes.Email, user.Email.ToString()),
                        }
                    ),
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
