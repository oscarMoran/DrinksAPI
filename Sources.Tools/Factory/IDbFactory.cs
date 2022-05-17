using System;
using System.Collections.Generic;
using System.Text;

namespace Sources.Tools.Factory
{
    public interface IDbFactory
    {
        public IDBManagement GetInstance();
    }
}
