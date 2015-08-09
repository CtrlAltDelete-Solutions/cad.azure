using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;


namespace CAD.Azure
{
    public interface INoSQLDatabase
    {
        string GetDatabaseName();
        string GetAuthKey();
        string GetEndPointUrl();
        INoSQL NoSqlClient();
        INoSQLCollection GetCollection(string collectionName);
        void Dispose();
    }
}
