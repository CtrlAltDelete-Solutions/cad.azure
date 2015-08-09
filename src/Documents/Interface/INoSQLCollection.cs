using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace CAD.Azure
{
    public interface INoSQLCollection
    {
        Task<Document> Insert(object documentToInsert);
        Document GetById(string Id);
        T GetById<T>(string Id);
        IOrderedQueryable<T> Query<T>();
        IQueryable<T> WhereQuery<T>(string whereQuery);
        IQueryable<T> Query<T>(string query);
        Task<Document> Update(object updatedDocument);
        bool Exists(string id);
        void Dispose();
    }
}
