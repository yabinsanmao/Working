using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Respository
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        string _connectionString;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public UserRepository(IConfiguration configuration)
        {
            string tstr = configuration.GetConnectionString("DefaultConnection");
            string iostr = System.IO.Directory.GetCurrentDirectory();
            _connectionString = String.Format(configuration.GetConnectionString("DefaultConnection"), System.IO.Directory.GetCurrentDirectory());
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserRole Login(string userName, string password)
        {
            using(var connection=new SqliteConnection(_connectionString))
            {
                var user = connection.Query<UserRole>("select users.*,roles.rolename from users join roles on users.roleid=roles.id where username=@username and password=@password",new { username=userName,password=password}).SingleOrDefault();
                if (user == null)
                {
                    throw new Exception("用户名或秘密那错误");
                }
                else
                {
                    return user;
                }
            }
        }
    }
}
