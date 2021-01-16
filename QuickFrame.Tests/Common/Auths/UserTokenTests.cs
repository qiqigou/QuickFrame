using System.Linq;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuickFrame.Tests;

namespace QuickFrame.Common.Tests
{
    [TestClass()]
    public class UserTokenTests : BaseTest
    {
        private readonly IUserToken _userToken;

        public UserTokenTests()
        {
            _userToken = GetRequiredService<IUserToken>();
        }

        [TestMethod()]
        public void CreateTest()
        {
            Claim[] claims =
            {
                new Claim(UserOptions.UserId,"001"),
                new Claim(UserOptions.Role,"admin"),
                new Claim(UserOptions.DBName,"zxsccore"),
                new Claim(UserOptions.UserName,"admin")
            };
            var token = _userToken.Create(claims);
            claims = _userToken.Decode(token);
            Assert.IsNotNull(claims);
            Assert.IsTrue(claims.Any());
        }
    }
}