//using Dapper;
using Infrastructure.Events;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
//using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EventStore
{
    /*
    public class SqlEventStore : IEventStore, IDisposable
    {
        const string INSERT_SQL =
@"DECLARE @Timestamp datetime2(7) = GETUTCDATE()
INSERT INTO [dbo].[EventStore] ([CommitId],[AggregateId],[Timestamp],[Version],[EventData]) VALUES (@CommitId ,@AggregateId,@Timestamp,@Version,@EventData)
";
        const string SELECT_SQL = 
@"SELECT CommitId, AggregateId, Version, EventData 
  FROM [dbo].[EventStore] 
  WHERE AggregateId = @AggregateId AND [Version] > @FromVersion 
  ORDER BY [Version]
";

        SqlConnection connection;

        public SqlEventStore(SqlConnection connection)
        {
            this.connection = connection;
        }

        private static JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Objects
        };

        public IEnumerable<IEvent> Get(Guid aggregateId, int fromVersion)
        {
            var eventsQuery = connection.Query<DocumentData>(SELECT_SQL, new { AggregateId = aggregateId, FromVersion = fromVersion });
            
            // create a function to deserialize event data
            Func<string, IEvent> func = (data) =>
            {
                var output = JsonConvert.DeserializeObject(data, settings);

                return (IEvent)output;
            };

            // deserialize the events from our documents
            var events = from e in eventsQuery
                         select func(e.EventData);

            // return the data as events
            return events.Cast<IEvent>();
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
            
            connection.Execute(INSERT_SQL, document);

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
    */
}
