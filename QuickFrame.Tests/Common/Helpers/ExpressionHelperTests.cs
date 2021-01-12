﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Models;
using QuickFrame.Tests;
using System;
using System.Linq.Expressions;
using System.Net;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class ExpressionHelperTests : BaseTest
    {
        [TestMethod()]
        public void WhereEqualOrTest()
        {
            var whereExp1 = ExpressionHelper.WhereEqualOr<userinfo_us, int>(new[] { nameof(userinfo_us.uid) }, new[] { 1, 98, 89 });
            Expression<Func<userinfo_us, bool>> whereExp2 = px => px.uid == 1 || px.uid == 98 || px.uid == 89;
            Assert.AreEqual(whereExp1.ToString(), whereExp2.ToString());
        }

        [TestMethod()]
        public void WhereLambdaTest()
        {
            var whereExp1 = ExpressionHelper.WhereLambda<userinfo_us, string>(new[] { nameof(userinfo_us.name) }, "wyl");
            Expression<Func<userinfo_us, bool>> whereExp2 = px => px.name == "wyl";
            Assert.AreEqual(whereExp1.ToString(), whereExp2.ToString());
        }

        [TestMethod()]
        public void MemberLambdaTest()
        {
            var member1 = ExpressionHelper.MemberLambda<userinfo_us, int>(new[] { nameof(userinfo_us.uid) });
            Expression<Func<userinfo_us, int>> member2 = px => px.uid;
            Assert.AreEqual(member1.ToString(), member2.ToString());
        }

        [TestMethod()]
        public void AssignLambdaTest()
        {
            var actionExp = ExpressionHelper.AssignLambda<userinfo_us, string>(nameof(userinfo_us.name));
            var user = new userinfo_us { name = "xxx" };
            actionExp.Compile().Invoke(user, "wyl");
            Assert.AreEqual(user.name, "wyl");
        }

        [TestMethod()]
        public void WhereEqualOrTest1()
        {
            var whereExp1 = ExpressionHelper.WhereEqualOr<userinfo_us, (string, string)>(new[] { nameof(userinfo_us.name), nameof(userinfo_us.sex) }, new[] { ("wyl", "男"), ("xxx", "女") });
            Expression<Func<userinfo_us, bool>> whereExp2 = px => (px.name == "wyl" && px.sex == "男") || (px.name == "xxx" && px.sex == "女");
            Assert.AreEqual(whereExp1.ToString(), whereExp2.ToString());
        }

        [TestMethod()]
        public void WhereLambdaTest1()
        {
            var whereExp1 = ExpressionHelper.WhereLambda<userinfo_us, (string, string)>(new[] { nameof(userinfo_us.name), nameof(userinfo_us.sex) }, ("wyl", "拉拉"));
            Expression<Func<userinfo_us, bool>> whereExp2 = px => px.name == "wyl" && px.sex == "拉拉";
            Assert.AreEqual(whereExp1.ToString(), whereExp2.ToString());
        }

        [TestMethod()]
        public void MemberLambdaTest1()
        {
            var member1 = ExpressionHelper.MemberLambda<userinfo_us, (string, string)>(new[] { nameof(userinfo_us.name), nameof(userinfo_us.sex) });
            Expression<Func<userinfo_us, (string, string)>> member2 = px => new ValueTuple<string, string>(px.name, px.sex);
            var user = new userinfo_us { name = "wyl", sex = "男" };
            Assert.AreEqual(member1.Compile().Invoke(user), member2.Compile().Invoke(user));
        }

        [TestMethod()]
        public void GetAnonymousNameTest()
        {
            var array = ExpressionHelper.GetMemberNames<userinfo_us, (string, int)>(px => new(px.name, px.age));
            Assert.AreEqual(array.Length, 2);
            Assert.AreEqual(array[0], "name");
            Assert.AreEqual(array[1], "age");
            array = ExpressionHelper.GetMemberNames<userinfo_us, object>(px => new { px.name, px.age });
            Assert.AreEqual(array.Length, 2);
            Assert.AreEqual(array[0], "name");
            Assert.AreEqual(array[1], "age");
            array = ExpressionHelper.GetMemberNames<userinfo_us, string>(px => px.name);
            Assert.AreEqual(array.Length, 1);
            Assert.AreEqual(array[0], "name");
            array = ExpressionHelper.GetMemberNames<userinfo_us, int>(px => px.age);
            Assert.AreEqual(array[0], "age");
        }

        [TestMethod()]
        public void Test()
        {
            var type = typeof(userinfo_us);
            var propName = nameof(userinfo_us.name);
            var lab = Expression.Label(typeof(string));
            var px = Expression.Parameter(type, "px");

            var key_var = Expression.Variable(typeof(string), "key");
            var prop = Expression.Property(px, type, propName);
            var assign = Expression.Assign(key_var, prop);
            var notequal = Expression.NotEqual(px, Expression.Default(type));
            var iftrue = Expression.IfThen(notequal, assign);

            var ret = Expression.Return(lab, key_var);
            var labExpr = Expression.Label(lab, Expression.Default(typeof(string)));
            var block = Expression.Block(new[] { key_var }, new Expression[] { iftrue, ret, labExpr });
            var express = Expression.Lambda<Func<userinfo_us?, string?>>(block, px);
            var func = express.Compile();

            var user = new userinfo_us { name = "wwww" };
            var key = func.Invoke(user);
            Assert.AreEqual(key, "wwww");
            user = default;
            key = func.Invoke(user);
            Assert.IsNull(key);
        }

        [TestMethod()]
        public void UrlTest()
        {
            var msg = "CNi3GvXDL%2FA=";
            msg = WebUtility.UrlDecode(msg);
            Assert.AreEqual(msg, "CNi3GvXDL/A=");
        }
    }
}