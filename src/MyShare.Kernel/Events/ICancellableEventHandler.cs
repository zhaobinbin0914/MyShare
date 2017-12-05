using MyShare.Kernel.Messages;

namespace MyShare.Kernel.Events
{
    /// <summary>
    /// Defines a handler for an event with a cancellation token.
    /// </summary>
    /// <typeparam name="T">Event type being handled</typeparam>
    public interface ICancellableEventHandler<in T> : ICancellableHandler<T> where T : IEvent
    {
    }
}