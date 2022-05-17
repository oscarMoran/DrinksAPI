using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks.Models
{
    public class UserResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
