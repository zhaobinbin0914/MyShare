using System;
using System.Collections.Generic;
using System.Linq;
using MyShare.Sample.ReadModel.Events;
using MyShare.Sample.WriteModel.Commands;
using MyShare.Sample.WriteModel.Domain;
using MyShare.Sample.WriteModel.Handlers;
using MyShare.Kernel.Events;
using Xunit;
using MyShare.Kernel.Tests.Extensions;

namespace MyShare.Sample.Test.WriteModel
{
    public class When_item_checked_in : Specification<InventoryItem, InventoryCommandHandlers, CheckInItemsToInventory>
    {
        private Guid _guid;
        protected override InventoryCommandHandlers BuildHandler()
        {
            return new InventoryCommandHandlers(Session);
        }

        protected override IEnumerable<IEvent> Given()
        {
            _guid = Guid.NewGuid();
            return new List<IEvent>
            {
                new InventoryItemCreated(_guid, "Jadda") {Version = 1},
                new ItemsCheckedInToInventory(_guid, 2) {Version = 2}
            };
        }

        protected override CheckInItemsToInventory When()
        {
            return new CheckInItemsToInventory(_guid, 2, 2);
        }

        [Then]
        public void Should_create_one_event()
        {
            Assert.Equal(1, PublishedEvents.Count);
        }

        [Then]
        public void Should_create_correct_event()
        {
            Assert.IsType<ItemsCheckedInToInventory>(PublishedEvents.First());
        }

        [Then]
        public void Should_save_have_correct_number_of_items()
        {
            Assert.Equal(2, ((ItemsCheckedInToInventory)PublishedEvents.First()).Count);
        }
    }
}
