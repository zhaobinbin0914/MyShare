using System;
using System.Threading.Tasks;
using MyShare.Kernel.Caching;
using MyShare.Kernel.Tests.Substitutes;
using Xunit;
// ReSharper disable InconsistentNaming
namespace MyShare.Kernel.Tests.Caching
{
    public class When_getting_fails
    {
        private MemoryCache _cache;
        private Guid _id;

        public When_getting_fails()
        {
            _cache = new MemoryCache();
            var testRep = new TestRepository();
            var rep = new CacheRepository(testRep, new TestInMemoryEventStore(), _cache);
            _id = Guid.NewGuid();
            testRep.Get<TestAggregate>(_id).Wait();
            testRep.Throw = true;
            try
            {
                rep.Get<TestAggregate>(_id).Wait();
            }
            catch (Exception) { }
        }

        [Fact]
        public async Task Should_evict_old_object_from_cache()
        {
            var aggregate = await _cache.Get(_id);
            Assert.Null(aggregate);
        }
    }
}
