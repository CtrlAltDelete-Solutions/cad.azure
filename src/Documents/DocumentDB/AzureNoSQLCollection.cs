using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace CAD.Azure
{
    public class AzureNoSQLCollection : INoSQLCollection, IDisposable
    {
        private readonly INoSQL _dbClient;
        private readonly string _collectionName;
        private DocumentCollection _collection;

        private const string _selectQueryFormat = "SELECT * from {0} t WHERE {1}";
        private const string _collectionNameFormat = "[COLLECTION]";

        public AzureNoSQLCollection(INoSQL dbClient, string databaseName, string collectionName)
        {
            this._collectionName = collectionName;
            this._dbClient = dbClient;
            this.InitializeAsync(databaseName, collectionName).Wait();
        }

        private async Task InitializeAsync(string databaseName, string collectionName)
        {
            Database db = await this._dbClient.Initialize(databaseName);
            this._collection = await this._dbClient.GetCollection(db, collectionName);
        }

        public async Task<Document> Insert(object documentToInsert)
        {
            return await this._dbClient.Insert(this._collection, documentToInsert);
        }

        public Document GetById(string Id)
        {
            return this._dbClient.GetById(this._collection, Id);
        }

        public T GetById<T>(string Id)
        {
            return this.WhereQuery<T>("t.id = \"" + Id + "\"").AsEnumerable().FirstOrDefault();
            //var document = this.GetById(Id);
            //return (T)Convert.ChangeType(document,typeof(T));
        }

        public IQueryable<T> WhereQuery<T>(string whereQuery)
        {
            string query = String.Format(_selectQueryFormat, this._collectionName, whereQuery);
            return this._dbClient.Query<T>(this._collection, query);
        }
        /// <summary>
        /// Query by string. use [COLLECTION] for the current collection name. For Example: "SELECT * FROM [COLLECTION] t"
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(string query)
        {
            string newQuery = query.Replace(_collectionNameFormat, this._collectionName);
            return this._dbClient.Query<T>(this._collection, query);
        }

        public IOrderedQueryable<T> Query<T>()
        {
            return this._dbClient.Query<T>(this._collection);
        }

        public async Task<Document> Update(INoSQLDocument updatedDocument)
        {
            //get id by reflection
            //var docLink = updatedDocument.GetType().GetProperty("DocumentLink").GetValue(updatedDocument, null);
            var docLink = updatedDocument.DocumentLink;

            if (!String.IsNullOrEmpty(docLink))
            {
                await this._dbClient.Update(docLink.ToString(), updatedDocument);
                
            }
            else
            {
                //var id = updatedDocument.GetType().GetProperty("Id").GetValue(updatedDocument, null);
                var id = updatedDocument.Id;
                if (!String.IsNullOrEmpty(id))
                {
                    var existingDoc = this.GetById(id);
                    if (existingDoc != null)
                    {
                        await this._dbClient.Update(existingDoc.SelfLink, updatedDocument);
                    }
                }
            }
            return null;
        }

        public bool Exists(string id)
        {
            return this._dbClient.Exists(this._collection, id);
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
                    this._collection = null;
                }
            }
            finally
            {

            }
        }

    }
}
