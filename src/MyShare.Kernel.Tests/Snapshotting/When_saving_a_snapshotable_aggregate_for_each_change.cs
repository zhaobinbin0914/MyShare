﻿using System.Threading.Tasks;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Events;
using MyShare.Kernel.Snapshotting;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;

namespace MyShare.Kernel.Tests.Snapshotting
{
    public class When_saving_a_snapshotable_aggregate_for_each_change
    {
        private TestInMemorySnapshotStore _snapshotStore;
	    private ISession _session;
	    private TestSnapshotAggregate _aggregate;

        public When_saving_a_snapshotable_aggregate_for_each_change()
        {
            IEventStore eventStore = new TestInMemoryEventStore();
            _snapshotStore = new TestInMemorySnapshotStore();
            var snapshotStrategy = new DefaultSnapshotStrategy();
            var repository = new SnapshotRepository(_snapshotStore, snapshotStrategy, new Repository(eventStore), eventStore);
            _session = new Session(repository);
            _aggregate = new TestSnapshotAggregate();

            for (var i = 0; i < 150; i++)
            {
                _session.Add(_aggregate);
                _aggregate.DoSomething();
                _session.Commit();
            }
        }

        [Fact]
        public void Should_snapshot_100th_change()
        {
            Assert.Equal(100, _snapshotStore.SavedVersion);
        }

        [Fact]
        public void Should_not_snapshot_first_event()
        {
            Assert.False(_snapshotStore.FirstSaved);
        }

        [Fact]
        public async Task Should_get_aggregate_back_correct()
        {
            Assert.Equal(150, (await _session.Get<TestSnapshotAggregate>(_aggregate.Id)).Number);
        }
    }
}