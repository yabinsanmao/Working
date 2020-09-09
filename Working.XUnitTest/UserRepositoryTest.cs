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
    [Trait("�û��ִ���","UserRepository")]
    public class UserRepositoryTest
    {        
        [Fact]
        public void Login_Default_Return()
        {
            var dbMock = new Mock<IDbConnection>();
            var userRepository = new UserRepository(dbMock.Object, "");
            var list = new List<UserRole>() { new UserRole { ID=1,Name="����ΰ",RoleID=1,PassWord="gsw",DepartmentID=1,UserName="gsw",RoleName="Leader"} };
            //ģ��Query
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
            //ģ��Query
            dbMock.SetupDapper(db => db.Query<UserRole>(It.IsAny<string>(), null, null, true, null, null)).Returns(list);
            var exc = Assert.Throws<Exception>(() => { userRepository.Login("gsw", "gsw"); });

            Assert.Contains("�û������������",exc.Message);
        }
    }
}
