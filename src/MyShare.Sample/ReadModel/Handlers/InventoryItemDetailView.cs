using System;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Sample.ReadModel.Dtos;
using MyShare.Sample.ReadModel.Events;
using MyShare.Sample.ReadModel.Infrastructure;
using MyShare.Kernel.Events;

namespace MyShare.Sample.ReadModel.Handlers
{
    public class InventoryItemDetailView : ICancellableEventHandler<InventoryItemCreated>,
        ICancellableEventHandler<InventoryItemDeactivated>,
        ICancellableEventHandler<InventoryItemRenamed>,
        ICancellableEventHandler<ItemsRemovedFromInventory>,
        ICancellableEventHandler<ItemsCheckedInToInventory>
    {
        public Task Handle(InventoryItemCreated message, CancellationToken token)
        {
            InMemoryDatabase.Details.Add(message.Id,
                new InventoryItemDetailsDto(message.Id, message.Name, 0, message.Version));
            return Task.CompletedTask;
        }

        public Task Handle(InventoryItemRenamed message, CancellationToken token)
        {
            var d = GetDetailsItem(message.Id);
            d.Name = message.NewName;
            d.Version = message.Version;
            return Task.CompletedTask;
        }

        private static InventoryItemDetailsDto GetDetailsItem(Guid id)
        {
            if (!InMemoryDatabase.Details.TryGetValue(id, out var dto))
            {
                throw new InvalidOperationException("did not find the original inventory this shouldnt happen");
            }
            return dto;
        }

        public Task Handle(ItemsRemovedFromInventory message, CancellationToken token)
        {
            var dto = GetDetailsItem(message.Id);
            dto.CurrentCount -= message.Count;
            dto.Version = message.Version;
            return Task.CompletedTask;
        }

        public Task Handle(ItemsCheckedInToInventory message, CancellationToken token)
        {
            var dto = GetDetailsItem(message.Id);
            dto.CurrentCount += message.Count;
            dto.Version = message.Version;
            return Task.CompletedTask;
        }

        public Task Handle(InventoryItemDeactivated message, CancellationToken token)
        {
            InMemoryDatabase.Details.Remove(message.Id);
            return Task.CompletedTask;
        }
    }
}
