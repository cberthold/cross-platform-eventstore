using Infrastructure.Domain.Exception;
using Infrastructure.Events;
using System;
using System.Collections.Generic;
using Infrastructure.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Todo.Mobile.Infrastructure.Common;

namespace Infrastructure.Domain
{
    public abstract class AggregateRoot
    {
        private readonly List<IEvent> _changes = new List<IEvent>();

        public virtual Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            lock (_changes)
            {
                return _changes.ToArray();
            }
        }

        public FlushResult FlushUncommitedChanges()
        {
            lock (_changes)
            {
                var changes = _changes.ToArray();
                Version = Version + _changes.Count;
                _changes.Clear();
                return new FlushResult(changes, Version);
            }
        }

        public void LoadFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history)
            {
                if (e.Version != Version + 1)
                    throw new EventsOutOfOrderException(e.Id);
                ApplyChange(e, false);
            }
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (_changes)
            {
                var factory = DependencyService.Get<IPrivateReflectionFactory>();
                factory.CallMethod(this, "Apply", @event);
                //dyn.Apply(@event);
                if (isNew)
                {
                    _changes.Add(@event);
                }
                else
                {
                    Id = @event.Id;
                    Version++;
                }
            }
        }

        public class FlushResult
        {
            public IEvent[] Changes { get; }
            public int Version { get; }

            public FlushResult(IEvent[] changes, int version)
            {
                Changes = changes;
                Version = version;
            }
        }
    }
}
