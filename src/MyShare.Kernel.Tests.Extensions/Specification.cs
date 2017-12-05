using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Domain.Exception;
using MyShare.Kernel.Events;
using MyShare.Kernel.Snapshotting;

namespace MyShare.Kernel.Tests.Extensions
{
    public abstract class Specification<TAggregate, THandler, TCommand> 
        where TAggregate: AggregateRoot
        where THandler : class
        where TCommand : ICommand
    {

        protected TAggregate Aggregate { get; set; }
        protected ISession Session { get; set; }
        protected abstract IEnumerable<IEvent> Given();
        protected abstract TCommand When();
        protected abstract THandler BuildHandler();

        protected Snapshot Snapshot { get; set; }
        protected IList<IEvent> EventDescriptors { get; set; }
        protected IList<IEvent> PublishedEvents { get; set; }

        protected Specification()
        {
            var eventpublisher = new SpecEventPublisher();
            var eventstorage = new SpecEventStorage(eventpublisher, Given().ToList());
            var snapshotstorage = new SpecSnapShotStorage(Snapshot);

            var snapshotStrategy = new DefaultSnapshotStrategy();
            var repository = new SnapshotRepository(snapshotstorage, snapshotStrategy, new Repository(eventstorage), eventstorage);
            Session = new Session(repository);
            Aggregate = GetAggregate().Result;

            dynamic handler = BuildHandler();
            switch (handler)
            {
                case ICancellableCommandHandler<TCommand> _:
                    handler.Handle(When(), new CancellationToken());
                    break;
                case ICommandHandler<TCommand> _:
                    handler.Handle(When());
                    break;
                default:
                    throw new InvalidCastException($"{nameof(handler)} is not a command handler of type {typeof(TCommand)}");
            }

            Snapshot = snapshotstorage.Snapshot;
            PublishedEvents = eventpublisher.PublishedEvents;
            EventDescriptors = eventstorage.Events;
        }

        private async Task<TAggregate> GetAggregate()
        {
            try
            {
                return await Session.Get<TAggregate>(Guid.Empty);
            }
            catch (AggregateNotFoundException)
            {
                return null;
            }
        }
    }

    internal class SpecSnapShotStorage : ISnapshotStore
    {
        public SpecSnapShotStorage(Snapshot snapshot)
        {
            Snapshot = snapshot;
        }

        public Snapshot Snapshot { get; set; }

        public Task<Snapshot> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Snapshot);
        }

        public Task Save(Snapshot snapshot, CancellationToken cancellationToken = default(CancellationToken))
        {
            Snapshot = snapshot;
            return Task.CompletedTask;
        }
    }

    internal class SpecEventPublisher : IEventPublisher
    {
        public SpecEventPublisher()
        {
            PublishedEvents = new List<IEvent>();
        }

        public Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent
        {
            PublishedEvents.Add(@event);
            return Task.CompletedTask;
        }

        public IList<IEvent> PublishedEvents { get; set; }
    }

    internal class SpecEventStorage : IEventStore
    {
        private readonly IEventPublisher _publisher;

        public SpecEventStorage(IEventPublisher publisher, List<IEvent> events)
        {
            _publisher = publisher;
            Events = events;
        }

        public List<IEvent> Events { get; set; }

        public Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            Events.AddRange(events);
            return Task.WhenAll(events.Select(evt =>_publisher.Publish(evt, cancellationToken)));
                
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var events = Events.Where(x => x.Id == aggregateId && x.Version > fromVersion);
            return Task.FromResult(events);
        }
    }
}
