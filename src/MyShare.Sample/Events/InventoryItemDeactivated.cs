using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.Events
{
    public class InventoryItemDeactivated : IEvent 
	{
        public InventoryItemDeactivated(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}