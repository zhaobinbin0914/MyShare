using System;
using MyShare.Kernel.Domain;

namespace MyShare.Kernel.Tests.Substitutes
{
    public class TestAggregateNoParameterLessConstructor : AggregateRoot
    {
        public TestAggregateNoParameterLessConstructor(int i, Guid? id = null)
        {
            Id = id ?? Guid.NewGuid();
        }

        public void DoSomething()
        {
            ApplyChange(new TestAggregateDidSomething());
        }
    }
}