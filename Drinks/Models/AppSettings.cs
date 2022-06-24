using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drinks.Models
{
    public class AppSettings
    {
        public string SecretKey { get; set; }
        public string PubKeyName { get; set; }
        public string PrivateKeyName { get; set; }
    }
}
