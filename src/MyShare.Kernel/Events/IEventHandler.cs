﻿using MyShare.Kernel.Messages;

namespace MyShare.Kernel.Events
{
    /// <summary>
    /// Defines a handler for an event.
    /// </summary>
    /// <typeparam name="T">Event type being handled</typeparam>
    public interface IEventHandler<in T> : IHandler<T> where T : IEvent
    {
    }
}
