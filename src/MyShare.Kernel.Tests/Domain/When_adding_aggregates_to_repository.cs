﻿using System;
using System.Threading.Tasks;
using MyShare.Kernel.Domain;
using MyShare.Kernel.Domain.Exception;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;

namespace MyShare.Kernel.Tests.Domain
{
    public class When_adding_aggregates_to_repository
    {
        private Session _session;

        public When_adding_aggregates_to_repository()
        {
            var eventStore = new TestInMemoryEventStore();
            _session = new Session(new Repository(eventStore));
        }

        [Fact]
        public async Task Should_throw_if_different_object_with_tracked_guid_is_added()
        {
            var aggregate = new TestAggregate(Guid.NewGuid());
            var aggregate2 = new TestAggregate(aggregate.Id);
            await _session.Add(aggregate);
            await Assert.ThrowsAsync<ConcurrencyException>(async () => await _session.Add(aggregate2));
        }

        [Fact]
        public async Task Should_not_throw_if_object_already_tracked()
        {
            var aggregate = new TestAggregate(Guid.NewGuid());
            await _session.Add(aggregate);
            await _session.Add(aggregate);
        }
    }
}