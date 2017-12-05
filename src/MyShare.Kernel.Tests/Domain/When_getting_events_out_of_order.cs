using System;
using System.Threading.Tasks;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Domain.Exception;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;

namespace MyShare.Kernel.Tests.Domain
{
    public class When_getting_events_out_of_order
    {
	    private ISession _session;

        public When_getting_events_out_of_order()
        {
            var eventStore = new TestEventStoreWithBugs();
            _session = new Session(new Repository(eventStore));
        }

        [Fact]
        public async Task Should_throw_concurrency_exception()
        {
            var id = Guid.NewGuid();
            await Assert.ThrowsAsync<EventsOutOfOrderException>(async () => await _session.Get<TestAggregate>(id, 3));
        }
    }
}