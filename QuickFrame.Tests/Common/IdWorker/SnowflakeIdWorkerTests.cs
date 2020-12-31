using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickFrame.Tests;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class SnowflakeIdWorkerTests : BaseTest
    {
        private readonly IIdWorker _idWorker;

        public SnowflakeIdWorkerTests()
        {
            _idWorker = _serviceProvider.GetRequiredService<IIdWorker>();
        }

        [TestMethod()]
        public void GetId64Test()
        {
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