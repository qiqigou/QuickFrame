using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Models;
using QuickFrame.Tests;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class ObjFilterConvertHelperTests : BaseTest
    {
        private readonly List<userinfo_us> _data;

        public ObjFilterConvertHelperTests()
        {
            _data = new List<userinfo_us>
            {
                new userinfo_us { uid = 122, name = "wwwwyl", age = 18, sex = "男", birthday = new DateTime(2020,10,20) },
                new userinfo_us { uid = 123, name = "weyl", age = 22, sex = "男", birthday = new DateTime(2020,10,19) },
                new userinfo_us { uid = 124, name = "wylxxxx", age = 34, sex = "男", birthday = new DateTime(2020,10,21) },
                new userinfo_us { uid = 125, name = "wyl", age = 88, sex = "男", birthday = new DateTime(2020,10,18) },
                new userinfo_us { uid = 126, name = "admin", age = 18, sex = "女", birthday = new DateTime(2020,10,18) },
                new userinfo_us { uid = 127, name = "hhhwyl", age = 88, sex = "女", birthday = new DateTime(2020,10,18) }
            };
        }

        [TestMethod()]
        public void ConvertConditionTest()
        {
            var condition1 = new GroupInput
            {
                Logic = ConstantOptions.LogicConstant.And,
                Items = new ItemInput[]
                {
                    new ItemInput{ Field = nameof(userinfo_us.name),Value = "wyl", Compare = ConstantOptions.CompareConstant.Contains },
                    new ItemInput{ Field = nameof(userinfo_us.age),Value = "18",Compare = ConstantOptions.CompareConstant.Equal },
                    new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-17",Compare = ConstantOptions.CompareConstant.Greater },
                    new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-21",Compare = ConstantOptions.CompareConstant.Less },
                }
            };
            var condition2 = new GroupInput
            {
                Logic = ConstantOptions.LogicConstant.Or,
                Groups = new GroupInput[]
                {
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.And,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.name),Value = "wyl", Compare = ConstantOptions.CompareConstant.Contains },
                            new ItemInput{ Field = nameof(userinfo_us.age),Value = "18",Compare = ConstantOptions.CompareConstant.Equal },
                        }
                    },
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.And,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-17",Compare = ConstantOptions.CompareConstant.Greater },
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-19",Compare = ConstantOptions.CompareConstant.Less },
                        }
                    }
                }
            };
            var condition3 = new GroupInput
            {
                Logic = ConstantOptions.LogicConstant.And,
                Groups = new GroupInput[]
                {
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.Or,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.name),Value = "wyl", Compare = ConstantOptions.CompareConstant.Contains },
                            new ItemInput{ Field = nameof(userinfo_us.age),Value = "18",Compare = ConstantOptions.CompareConstant.Equal },
                        }
                    },
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.Or,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-17",Compare = ConstantOptions.CompareConstant.Greater },
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-19",Compare = ConstantOptions.CompareConstant.Less },
                        }
                    }
                }
            };
            var condition4 = new GroupInput
            {
                Logic = ConstantOptions.LogicConstant.And,
                Items = new ItemInput[]
                {
                    new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-17",Compare = ConstantOptions.CompareConstant.Greater },
                    new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-19",Compare = ConstantOptions.CompareConstant.Less },
                },
                Groups = new GroupInput[]
                {
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.Or,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.name),Value = "wyl", Compare = ConstantOptions.CompareConstant.Contains },
                            new ItemInput{ Field = nameof(userinfo_us.age),Value = "18",Compare = ConstantOptions.CompareConstant.Equal },
                        }
                    }
                }
            };

            int count = default;
            var query1 = _data.AsQueryable().QueryByGroupInput(condition1);
            var query2 = _data.AsQueryable().QueryByGroupInput(condition2);
            var query3 = _data.AsQueryable().QueryByGroupInput(condition3);
            var query4 = _data.AsQueryable().QueryByGroupInput(condition4);

            var filter1 = query1.ToList();
            count = _data.Where(x => x.name.Contains("wyl") && x.age == 18 && x.birthday > new DateTime(2020, 10, 17) && x.birthday < new DateTime(2020, 10, 21)).Count();
            Assert.IsTrue(filter1.Count == count);
            query1 = _data.AsQueryable().QueryByGroupInput(condition1);
            filter1 = query1.ToList();
            Assert.IsTrue(filter1.Count == count);

            var filter2 = query2.ToList();
            count = _data.Where(x => (x.name.Contains("wyl") && x.age == 18) || (x.birthday > new DateTime(2020, 10, 17) && x.birthday < new DateTime(2020, 10, 19))).Count();
            Assert.IsTrue(filter2.Count == count);
            query2 = _data.AsQueryable().QueryByGroupInput(condition2);
            filter2 = query2.ToList();
            Assert.IsTrue(filter2.Count == count);

            var filter3 = query3.ToList();
            count = _data.Where(x => (x.name.Contains("wyl") || x.age == 18) && (x.birthday > new DateTime(2020, 10, 17) || x.birthday < new DateTime(2020, 10, 19))).Count();
            Assert.IsTrue(filter3.Count == count);
            query3 = _data.AsQueryable().QueryByGroupInput(condition3);
            filter3 = query3.ToList();
            Assert.IsTrue(filter3.Count == count);

            var filter4 = query4.ToList();
            count = _data.Where(x => x.birthday > new DateTime(2020, 10, 17) && x.birthday < new DateTime(2020, 10, 19) && (x.name.Contains("wyl") || x.age == 18)).Count();
            Assert.IsTrue(filter4.Count == count);
            query4 = _data.AsQueryable().QueryByGroupInput(condition4);
            filter4 = query4.ToList();
            Assert.IsTrue(filter4.Count == count);
        }

        [TestMethod()]
        public void ConvertPageTest()
        {
            var page = new PageInput
            {
                Index = 1,
                Size = 2,
                Sort =
                new SortInput[]
                {
                    new SortInput
                    {
                        Desc = true,
                        OrderBy = nameof(userinfo_us.age),
                    },
                    new SortInput
                    {
                        Desc = false,
                        OrderBy = nameof(userinfo_us.name),
                    }
                }
            };
            var pageList = _data.AsQueryable().QueryByPageInput(page).ToList();
            Assert.IsTrue(pageList.Count == 2);
            Assert.AreEqual(pageList[0], _data[5]);

            pageList = _data.AsQueryable().QueryByPageInput(page).ToList();
            Assert.IsTrue(pageList.Count == 2);
            Assert.AreEqual(pageList[0], _data[5]);
        }

        [TestMethod()]
        public void ConvertToQueryableTest()
        {
            var condition = new GroupInput
            {
                Logic = ConstantOptions.LogicConstant.Or,
                Groups = new GroupInput[]
                {
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.And,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.name),Value = "wyl", Compare = ConstantOptions.CompareConstant.Contains },
                            new ItemInput{ Field = nameof(userinfo_us.age),Value = "18",Compare = ConstantOptions.CompareConstant.Equal },
                        }
                    },
                    new GroupInput
                    {
                        Logic = ConstantOptions.LogicConstant.And,
                        Items = new ItemInput[]
                        {
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-17 00:00:01",Compare = ConstantOptions.CompareConstant.Greater },
                            new ItemInput{ Field = nameof(userinfo_us.birthday),Value = "2020-10-19",Compare = ConstantOptions.CompareConstant.Less },
                        }
                    }
                }
            };
            var page = new PageInput
            {
                Index = 1,
                Size = 2,
                Sort =
                new SortInput[]
                {
                    new SortInput
                    {
                        Desc = true,
                        OrderBy = nameof(userinfo_us.age),
                    },
                    new SortInput
                    {
                        Desc = false,
                        OrderBy = nameof(userinfo_us.name),
                    }
                }
            };

            var list = _data.AsQueryable().QueryByGroupInput(condition).QueryByPageInput(page).ToList();
            Assert.IsTrue(list.Count == 2);
            Assert.AreEqual(list[0], _data[5]);

            list = _data.AsQueryable().QueryByGroupInput(new GroupInput()).QueryByPageInput(new PageInput()).ToList();
            Assert.IsTrue(list.Count == 6);
        }

        [TestMethod()]
        public void ConvertSortTest()
        {
            var sorts = new SortInput[]
            {
                new SortInput{  OrderBy = nameof(userinfo_us.name),Desc = true },
                new SortInput{  OrderBy = nameof(userinfo_us.age),Desc = true },
                new SortInput{  OrderBy = nameof(userinfo_us.birthday),Desc = false },
            };
            var query1 = ObjFilterConvertHelper.OrderBySortInput(_data.AsQueryable(), sorts).ToArray();
            var query2 = _data.AsQueryable().OrderByDescending(px => px.name).ThenByDescending(px => px.age).ThenBy(px => px.birthday).ToArray();
            for (int i = 0; i < query1.Length; i++)
            {
                Assert.AreEqual(query1[i].name, query2[i].name);
            }
        }
    }
}