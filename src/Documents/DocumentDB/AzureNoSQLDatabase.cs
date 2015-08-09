using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace CAD.Azure
{
    public class AzureNoSQLDatabase : INoSQLDatabase, IDisposable
    {
        private readonly string _endPointUrl;
        private readonly string _authKey;
        private readonly string _databasename;
        private readonly INoSQL _dbClient;
        private Database _database;
        
        public AzureNoSQLDatabase(string endpointUrl, string authKey, string databaseName)
        {
            this._authKey = authKey;
            this._endPointUrl = endpointUrl;
            this._databasename = databaseName;
            this._dbClient = new AzureNoSQL(this._endPointUrl, this._authKey);
            this.InitializeAsync(databaseName).Wait();
            
        }

        private async Task<Database> InitializeAsync(string databaseName)
        {
            if(this._database == null)
            {
                this._database = await this._dbClient.Initialize(databaseName);
            }
            return this._database;
        }

        public INoSQLCollection GetCollection(string collectionName)
        {
            AzureNoSQLCollection collection = new AzureNoSQLCollection(this._dbClient, this._databasename, collectionName);

            return collection;
        }

        public string GetDatabaseName()
        {
            return this._databasename;
        }

        public string GetAuthKey()
        {
            return this._authKey;
        }

        public string GetEndPointUrl()
        {
            return this._endPointUrl;
        }

        public INoSQL NoSqlClient()
        {
            return this._dbClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                if (isDisposing)
                {
                    this._dbClient.Dispose();
                }
            }
            finally
            {

            }
        }

    }
}
