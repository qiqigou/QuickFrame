using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Tests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class RedisIdWorkerTests : BaseTest
    {
        private readonly IIdWorker _idWorker;
        private readonly CacheConfig _cacheConfig;

        public RedisIdWorkerTests()
        {
            _idWorker = GetRequiredService<IIdWorker>();
            _cacheConfig = GetRequiredService<IOptions<CacheConfig>>().Value;
        }

        [TestMethod()]
        public void GetId64Test()
        {
            if (_appConfig.IdWorkerProvid is not IdWorkerProvidType.Redis) return;
            if (_cacheConfig.Type is not CacheType.Redis) return;

            var list = new List<long>(10_0000);
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var id = _idWorker.GetId64();
                        list.Add(id);
                    }
                });
            }
            Task.WaitAll(tasks);
            var set = new HashSet<long>(list);
            if (set.Count != list.Count) Assert.Fail();
        }

        [TestMethod()]
        public void GetIdStringTest()
        {
            if (_appConfig.IdWorkerProvid is not IdWorkerProvidType.Redis) return;
            if (_cacheConfig.Type is not CacheType.Redis) return;

            var list = new List<string>(10_0000);
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var id = _idWorker.GetIdString();
                        list.Add(id);
                    }
                });
            }
            Task.WaitAll(tasks);
            var set = new HashSet<string>(list);
            if (set.Count != list.Count) Assert.Fail();
        }
    }
}