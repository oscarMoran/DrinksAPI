using System;
using System.Collections.Generic;
using System.Text;

namespace Sources.Tools.Factory
{
    public class DbFactory : IDbFactory
    {
        private  readonly string _connStr;

        public DbFactory(string connStr)
        {
            _connStr = connStr;
        }
        public IDBManagement GetInstance()
        {
            return new DBManagementMySql(_connStr);
        }
    }
}
