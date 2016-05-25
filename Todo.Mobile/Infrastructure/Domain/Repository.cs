﻿using Infrastructure.Domain.Exception;
using Infrastructure.Domain.Factories;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Domain
{
    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        private readonly IEventPublisher _publisher;
        private readonly IAggregateFactory _aggregateFactory;

        public Repository(IEventStore eventStore, IEventPublisher publisher, IAggregateFactory aggregateFactory)
        {
            if (eventStore == null)
                throw new ArgumentNullException(nameof(eventStore));
            if (publisher == null)
                throw new ArgumentNullException(nameof(publisher));
            if (aggregateFactory == null)
                throw new ArgumentNullException(nameof(aggregateFactory));

            _eventStore = eventStore;
            _publisher = publisher;
            _aggregateFactory = aggregateFactory;

        }

        public void Save<T>(T aggregate, int? expectedVersion = null) where T : AggregateRoot
        {
            if (expectedVersion != null && _eventStore.Get(aggregate.Id, expectedVersion.Value).Any())
                throw new ConcurrencyException(aggregate.Id);
            var i = 0;
            var flushresult = aggregate.FlushUncommitedChanges();
            foreach (var @event in flushresult.Changes)
            {
                if (@event.Id == Guid.Empty && aggregate.Id == Guid.Empty)
                    throw new AggregateOrEventMissingIdException(aggregate.GetType(), @event.GetType());
                if (@event.Id == Guid.Empty)
                    @event.Id = aggregate.Id;

                i++;
                @event.Version = flushresult.Version - flushresult.Changes.Length + i;
                @event.TimeStamp = DateTimeOffset.UtcNow;
                _eventStore.Save(@event);
                _publisher.Publish(@event);
            }
        }

        public T Get<T>(Guid aggregateId) where T : AggregateRoot
        {
            return LoadAggregate<T>(aggregateId);
        }

        private T LoadAggregate<T>(Guid id) where T : AggregateRoot
        {
            var aggregate = _aggregateFactory.CreateAggregate<T>();

            var events = _eventStore.Get(id, -1);
            if (!events.Any())
                throw new AggregateNotFoundException(id);

            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}
