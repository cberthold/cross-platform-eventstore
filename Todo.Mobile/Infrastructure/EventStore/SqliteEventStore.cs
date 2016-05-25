using Infrastructure.Common;
using Infrastructure.Events;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Mobile.Infrastructure.Common;

namespace Todo.Mobile.Infrastructure.EventStore
{
    public class SqliteEventStore : IEventStore, IDisposable
    {

        const string INSERT_SQL =
@"INSERT INTO DocumentData ([CommitId],[AggregateId],[Timestamp],[Version],[EventData]) VALUES (@CommitId ,@AggregateId,@Timestamp,@Version,@EventData)
";
        const string SELECT_SQL =
@"SELECT CommitId, AggregateId, Version, EventData 
  FROM DocumentData 
  WHERE AggregateId = @AggregateId AND [Version] > @FromVersion 
  ORDER BY [Version]
";


        SQLiteConnection connection;

        public SqliteEventStore(ISQLite sqlLite)
        {
            this.connection = sqlLite.GetConnection(Database.Write);
        }
        
        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            var eventsQuery = connection.Table<DocumentData>().Where((doc) => doc.AggregateId == aggregateId && doc.Version > fromVersion);
            
            // create a function to deserialize event data
            Func<string, IEvent> func = (data) =>
            {
                var output = JsonConvert.DeserializeObject(data, settings);

                return (IEvent)output;
            };

            var eventsList = eventsQuery.ToList();

            // deserialize the events from our documents
            var events = from e in eventsList
                         select func(e.EventData);

            // return the data as events
            return events.OfType<IEvent>();
        }

        public void Save(IEvent @event)
        {
            var document = new DocumentData()
            {
                CommitId = Guid.NewGuid(),
                AggregateId = @event.Id,
                Version = @event.Version,
                EventData = JsonConvert.SerializeObject(@event, settings)
            };

            connection.Insert(document);

        }

        public class DocumentData
        {
            public Guid CommitId { get; set; }
            public Guid AggregateId { get; set; }
            public int Version { get; set; }
            public string EventData { get; set; }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    if (connection != null)
                        connection.Dispose();

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                connection = null;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~SqlEventStore() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
