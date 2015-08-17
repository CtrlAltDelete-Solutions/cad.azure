using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;


namespace CAD.Azure
{
    public class AzureNoSQL : INoSQL, IDisposable
    {
        private readonly DocumentClient _dbClient;

        public DocumentClient DBClient
        {
            get
            {
                return this._dbClient;
            }
        }

        public AzureNoSQL(string endPointUrl, string authKey)
        {
            Uri endPoint = new Uri(endPointUrl);
            this._dbClient = new DocumentClient(endPoint, authKey);
        }

        public async Task<Database> Initialize(string databaseName)
        {

            var database = this._dbClient.CreateDatabaseQuery().Where(db => db.Id == databaseName).AsEnumerable().FirstOrDefault();
            

            if (database == null)
            {
                database = await this._dbClient.CreateDatabaseAsync(new Database { Id = databaseName });
                
            }

            return database;

        }

        public async Task<DocumentCollection> GetCollection(Database database, string collectionName)
        {
            return await this.GetCollection(database.SelfLink, collectionName);
        }

        public async Task<DocumentCollection> GetCollection(string databaseSelfLink, string collectionName)
        {
            var collection = this._dbClient.CreateDocumentCollectionQuery(databaseSelfLink).Where(c => c.Id == collectionName).ToArray().FirstOrDefault();

            if(collection == null)
            {
                collection = await this._dbClient.CreateDocumentCollectionAsync(databaseSelfLink, new DocumentCollection { Id = collectionName });
            }

            
            return collection;
        }


        public async Task<Document> Insert(DocumentCollection collection, object documentToInsert)
        {
            return await this.Insert(collection.SelfLink, documentToInsert);

        }

        public async Task<Document> Insert(string collectionSelfLink, object documentToInsert)
        {
            var document = await this._dbClient.CreateDocumentAsync(collectionSelfLink, documentToInsert);
            return document;
        }


        public Document GetById(DocumentCollection collection, string Id)
        {
            return this.GetById(collection.DocumentsLink, Id);
        }

        public Document GetById(string collectionDocumentLink, string Id)
        {
            return this._dbClient.CreateDocumentQuery(collectionDocumentLink).Where(d => d.Id == Id).Select(s => s).AsEnumerable().FirstOrDefault();
        }

        public IOrderedQueryable<T> Query<T>(DocumentCollection collection)
        {
            return this._dbClient.CreateDocumentQuery<T>(collection.DocumentsLink);

            
        }

        public IQueryable<T> Query<T>(DocumentCollection collection, string query)
        {
            return this._dbClient.CreateDocumentQuery<T>(collection.DocumentsLink,query);


        }

        public Task<Document> Update(Document document, object updatedDocument)
        {
            return this.Update(document.SelfLink, updatedDocument);
        }

        public async Task<Document> Update(string documentSelfLink, object updatedDocument)
        {
            return await this._dbClient.ReplaceDocumentAsync(documentSelfLink, updatedDocument);
        }

        public bool Exists(DocumentCollection collection, string id)
        {
            return this.Exists(collection.DocumentsLink, id);
        }

        public bool Exists(string collectionSelfLink, string id)
        {
            var document = this.GetById(collectionSelfLink, id);
            return (document != null);
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
                if(isDisposing)
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
