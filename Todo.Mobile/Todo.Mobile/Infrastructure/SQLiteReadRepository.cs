using Infrastructure.Common;
using Infrastructure.Repository;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Mobile.Infrastructure.Common;

namespace Todo.Mobile.Infrastructure
{
    public class SQLiteReadRepository<T> : IReadRepository<T>, IDisposable
        where T : class, IEntity, new()
    {

        SQLiteConnection connection;
        SQLiteAsyncConnection asyncConnection;

        public SQLiteReadRepository(ISQLite sqlLite)
        {
            connection = sqlLite.GetConnection(Database.Read);
            asyncConnection = sqlLite.GetAsyncConnection(Database.Read);
        }

        public T GetById(Guid id)
        {
            return connection.Get<T>(id);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return asyncConnection.GetAsync<T>(id);
        }

        public IQueryable<T> GetCollection()
        {
            return connection.Table<T>().AsQueryable();
        }

        public void Insert(T document)
        {
            connection.Insert(document);
        }

        public Task InsertAsync(T document)
        {
            return asyncConnection.InsertAsync(document);
        }

        public void Update(T document)
        {
            connection.Update(document);
        }

        public Task UpdateAsync(T document)
        {
            return asyncConnection.UpdateAsync(document);
        }

        public void Dispose()
        {
            if(connection != null)
            {
                connection.Dispose();
                connection = null;
            }

            asyncConnection = null;
        }
    }
}
