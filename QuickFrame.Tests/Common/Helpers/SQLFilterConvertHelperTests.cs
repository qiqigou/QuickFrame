using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Tests;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class SQLFilterConvertHelperTests : BaseTest
    {
        [TestMethod()]
        public void SqlConvertExpressionTest()
        {
            var sql = "Name = 'a.b.('";
            var exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);

            sql = "Name = 'a.b.(\\''  ";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);

            sql = "Time >= '2020-11-19T18:51:08.407' ";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);

            sql = "Name='' and Age> 18";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
            Expression<Func<UserDto, bool>> exp2 = px => px.Name == "" && px.Age > 18;
            Assert.AreEqual(exp1.ToString(), exp2.ToString());

            sql = "( Name ='wyl' and (Age>=18 or Age<22) or (( Name.contains('xxxy') or Name= 'wyl' ) and (Age > 18 or Age <=50)))";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
            exp2 = px => (px.Name == "wyl" && (px.Age >= 18 || px.Age < 22) || ((px.Name!.Contains("xxxy") || px.Name == "wyl") && (px.Age > 18 || px.Age <= 50)));
            Assert.AreEqual(exp1.ToString(), exp2.ToString());

            sql = "Name.contains('') and Age> 22";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
            exp2 = px => px.Name.Contains("") && px.Age > 22;
            Assert.AreEqual(exp1.ToString(), exp2.ToString());

            sql = "( Name= 'wyl'      and  (Age>=18 or Age<22) or  ( ( Name.contains('xxxy')  or Name= 'wyl' ) and (Age > 18 or Age<=50   ))  )";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
            sql = "(Name='' and Age>18 or    Age=98  )";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
            sql = "Name='' and Age>18";
            exp1 = SQLFilterConvertHelper.ConvertToExpression<UserDto>(sql);
        }

        public class UserDto : IMEntity
        {
            public string Name { get; set; } = string.Empty;
            public int Age { get; set; }
            public DateTime? Time { get; set; }
        }
    }
}