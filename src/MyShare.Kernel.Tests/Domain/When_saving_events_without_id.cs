﻿using System;
using System.Threading.Tasks;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Domain.Exception;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;

namespace MyShare.Kernel.Tests.Domain
{
    public class When_saving_events_without_id
    {
        private TestInMemoryEventStore _eventStore;
        private TestAggregate _aggregate;
        private Repository _rep;

        public When_saving_events_without_id()
        {
            _eventStore = new TestInMemoryEventStore();
            _rep = new Repository(_eventStore);

            _aggregate = new TestAggregate(Guid.Empty);
        }

        [Fact]
        public async Task Should_throw_aggregate_or_event_missing_id_exception_from_repository()
        {
            await Assert.ThrowsAsync<AggregateOrEventMissingIdException>(async () => await _rep.Save(_aggregate, 0));
        }
    }
}