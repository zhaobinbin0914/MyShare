using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Messages;
using MyShare.Kernel.Routing;

namespace MyShare.Kernel.Tests.Substitutes
{
    public class TestHandleRegistrar : IHandlerRegistrar
    {
        public static readonly List<TestHandlerListItem> HandlerList = new List<TestHandlerListItem>();

        public void RegisterHandler<T>(Func<T, CancellationToken, Task> handler) where T : class, IMessage
        {
            HandlerList.RemoveAll(x => x.Type == typeof(T));
            HandlerList.Add(new TestHandlerListItem {Type = typeof(T), Handler = handler});
        }
    }

    public class TestHandlerListItem
    {
        public Type Type;
        public dynamic Handler;
    }
}
