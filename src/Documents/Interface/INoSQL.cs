using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace CAD.Azure
{
    public interface INoSQL
    {
        Task<Database> Initialize(string databaseName);
        Task<DocumentCollection> GetCollection(Database database, string collectionName);
        Task<DocumentCollection> GetCollection(string databaseSelfLink, string collectionName);
        Task<Document> Insert(DocumentCollection collection, object documentToInsert);
        Task<Document> Insert(string collectionSelfLink, object documentToInsert);
        Document GetById(DocumentCollection collection, string Id);
        Document GetById(string collectionDocumentLink, string Id);
        IOrderedQueryable<T> Query<T>(DocumentCollection collection);
        IQueryable<T> Query<T>(DocumentCollection collection, string query);
        Task<Document> Update(Document document, object updatedDocument);
        Task<Document> Update(string documentSelfLink, object updatedDocument);
        bool Exists(DocumentCollection collection, string id);
        bool Exists(string collectionSelfLink, string id);
        void Dispose();
    }
}

