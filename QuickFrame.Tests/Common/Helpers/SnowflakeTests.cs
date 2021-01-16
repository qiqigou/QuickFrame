using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Tests;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class SnowflakeTests : BaseTest
    {
        [TestMethod()]
        public void NextIdTest()
        {
            var snowflake = new Snowflake(10, 21);

            var list = new List<long>(10_0000);
            var tasks = new Task[100];
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        var id = snowflake.NextId();
                        list.Add(id);
                    }
                });
            }
            Task.WaitAll(tasks);
            var set = new HashSet<long>(list);
            if (set.Count != list.Count) Assert.Fail();
        }
    }
}