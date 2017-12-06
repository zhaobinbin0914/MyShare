using System;
using MyShare.Kernel.Events;

namespace MyShare.Sample.Events
{
    public class InventoryItemRenamed : IEvent
    {
        public readonly string NewName;
 
        public InventoryItemRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}