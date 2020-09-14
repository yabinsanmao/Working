using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Respository
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// 连接对象
        /// </summary>
        IDbConnection _dbConnection;
        public UserRepository(IDbConnection dbConnection, string connectionString)
        {

            dbConnection.ConnectionString = connectionString;
            _dbConnection = dbConnection;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserRole Login(string userName, string password)
        {

            var user = _dbConnection.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where username=@username and password=@password", new { username = userName, password = password }).SingleOrDefault();
            if (user == null)
            {
                throw new Exception("用户名或密码错误！");
            }
            else
            {
                return user;
            }
        }

    }
}
