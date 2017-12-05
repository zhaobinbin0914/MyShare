using System;

namespace MyShare.Kernel.Domain.Exception
{
    public class EventsOutOfOrderException : System.Exception
    {
        public EventsOutOfOrderException(Guid id)
            : base($"Eventstore gave event for aggregate {id} out of order")
        { }
    }
}