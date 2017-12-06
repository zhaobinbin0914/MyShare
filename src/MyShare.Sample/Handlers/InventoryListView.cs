using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Events;
using MyShare.Sample.Dtos;
using MyShare.Sample.Events;
using MyShare.Sample.Infrastructure;

namespace MyShare.Sample.Handlers
{
	public class InventoryListView : ICancellableEventHandler<InventoryItemCreated>,
	    ICancellableEventHandler<InventoryItemRenamed>,
	    ICancellableEventHandler<InventoryItemDeactivated>
    {
        public Task Handle(InventoryItemCreated message, CancellationToken token)
        {
            InMemoryDatabase.List.Add(new InventoryItemListDto(message.Id, message.Name));
            return Task.CompletedTask;
        }

        public Task Handle(InventoryItemRenamed message, CancellationToken token)
        {
            var item = InMemoryDatabase.List.Find(x => x.Id == message.Id);
            item.Name = message.NewName;
            return Task.CompletedTask;
        }

        public Task Handle(InventoryItemDeactivated message, CancellationToken token)
        {
            InMemoryDatabase.List.RemoveAll(x => x.Id == message.Id);
            return Task.CompletedTask;
        }
    }
}