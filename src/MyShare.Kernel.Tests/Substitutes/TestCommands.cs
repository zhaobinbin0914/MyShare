using System;
using System.Threading;
using System.Threading.Tasks;
using MyShare.Kernel.Commands;
using MyShare.Kernel.Domain.Exception;

namespace MyShare.Kernel.Tests.Substitutes
{
    public class TestAggregateDoSomething : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public bool LongRunning { get; set; }
    }

    public class TestAggregateDoSomethingElse : ICommand
    {
        public Guid Id { get; set; }
        public int ExpectedVersion { get; set; }
        public bool LongRunning { get; set; }
    }

    public class TestAggregateDoSomethingHandler : ICancellableCommandHandler<TestAggregateDoSomething>
    {
        public async Task Handle(TestAggregateDoSomething message, CancellationToken token)
        {
            if (message.LongRunning)
                await Task.Delay(50, token);
            if(message.ExpectedVersion != TimesRun)
                throw new ConcurrencyException(message.Id);
            TimesRun++;
            Token = token;
        }

        public int TimesRun { get; set; }
        public CancellationToken Token { get; set; }

    }
	public class TestAggregateDoSomethingElseHandler : AbstractTestAggregateDoSomethingElseHandler
    {
        public override Task Handle(TestAggregateDoSomethingElse message)
        {
            TimesRun++;
            return Task.CompletedTask;
        }

        public int TimesRun { get; set; }
    }

    public abstract class AbstractTestAggregateDoSomethingElseHandler : ICommandHandler<TestAggregateDoSomethingElse>
    {
        public abstract Task Handle(TestAggregateDoSomethingElse message);
    }
}