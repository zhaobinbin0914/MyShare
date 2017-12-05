using System;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Snapshotting;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;

namespace MyShare.Kernel.Tests.Snapshotting
{
    public class When_getting_not_snapshotable_aggreate
    {
        private TestSnapshotStore _snapshotStore;
        private TestAggregate _aggregate;

        public When_getting_not_snapshotable_aggreate()
        {
            var eventStore = new TestEventStore();
            _snapshotStore = new TestSnapshotStore();
            var snapshotStrategy = new DefaultSnapshotStrategy();
            var repository = new SnapshotRepository(_snapshotStore, snapshotStrategy, new Repository(eventStore), eventStore);
            var session = new Session(repository);

            _aggregate = session.Get<TestAggregate>(Guid.NewGuid()).Result;
        }

        [Fact]
        public void Should_not_ask_for_snapshot()
        {
            Assert.False(_snapshotStore.VerifyGet);
        }

        [Fact]
        public void Should_restore_from_events()
        {
            Assert.Equal(3, _aggregate.Version);
        }
    }
}
