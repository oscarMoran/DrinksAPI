using Drinks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks.Services
{
    public interface IUserService
    {
        public Task<UserResponse> Auth(UserRequest model);
    }
}
