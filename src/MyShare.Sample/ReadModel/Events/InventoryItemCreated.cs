using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.ReadModel.Events
{
    public class InventoryItemCreated : IEvent 
	{
        public readonly string Name;
        public InventoryItemCreated(Guid id, string name) 
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
	}
}