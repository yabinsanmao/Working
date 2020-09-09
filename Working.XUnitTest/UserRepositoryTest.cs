using Dapper;
using Moq;
using Moq.Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Working.Models.DataModel;
using Working.Models.Respository;
using Xunit;

namespace Working.XUnitTest
{
    [Trait("用户仓储层","UserRepository")]
    public class UserRepositoryTest
    {        
        [Fact]
        public void Login_Default_Return()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");
            var list = new List<UserRole>() { new UserRole { ID=1,Name="桂素伟",RoleID=1,PassWord="gsw",DepartmentID=1,UserName="gsw",RoleName="Leader"} };
            //模拟Query
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var userRole = userRepository.Login("gsw", "gsw");
            Assert.NotNull(userRole);
        }

        [Fact]
        public void Login_Null_Exception()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");
            var list = new List<UserRole>();
            //模拟Query
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var exc = Assert.Throws<Exception>(() => { userRepository.Login("gsw", "gsw"); });

            Assert.Contains("用户名或密码错误！",exc.Message);
        }
    }
}
