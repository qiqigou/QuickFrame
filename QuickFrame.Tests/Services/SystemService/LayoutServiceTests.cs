﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using QuickFrame.Model;
using QuickFrame.Tests;

namespace QuickFrame.Service.Tests
{
    [TestClass()]
    public class LayoutServiceTests : BaseTest
    {
        private readonly ILayoutService _layoutService;

        public LayoutServiceTests()
        {
            _layoutService = GetRequiredService<ILayoutService>();
        }

        [TestMethod()]
        public async Task QueryViewColumnsTest()
        {
            var columns1 = (await _layoutService.QueryViewColumnsAsync(nameof(sysclassdtl_cd))).ToArray();
            Assert.IsTrue(columns1.Any());
        }
    }
}