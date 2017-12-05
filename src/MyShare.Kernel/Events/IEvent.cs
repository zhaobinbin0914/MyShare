using MyShare.Kernel.Messages;
using System;

namespace MyShare.Kernel.Events
{
    /// <summary>
    /// Defines an event with required fields.
    /// </summary>
    public interface IEvent : IMessage
    {
        Guid Id { get; set; }
        int Version { get; set; }
        DateTimeOffset TimeStamp { get; set; }
    }
}
