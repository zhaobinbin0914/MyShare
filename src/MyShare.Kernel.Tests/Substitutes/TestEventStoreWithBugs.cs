using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Events;

namespace MyShare.Kernel.Tests.Substitutes
{
    public class TestEventStoreWithBugs : IEventStore
    {
        public Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int version, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (aggregateId == Guid.Empty)
            {
                return Task.FromResult((IEnumerable<IEvent>)new List<IEvent>());
            }

            return Task.FromResult((IEnumerable<IEvent>) new List<IEvent>
            {
                new TestAggregateDidSomething {Id = aggregateId, Version = 3},
                new TestAggregateDidSomething {Id = aggregateId, Version = 2},
                new TestAggregateDidSomethingElse {Id = aggregateId, Version = 1}
            });
        }
    }
}