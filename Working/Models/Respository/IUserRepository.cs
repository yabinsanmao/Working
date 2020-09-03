using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Working.Models.DataModel;

namespace Working.Models.Respository
{
    public interface IUserRepository
    {
        UserRole Login(string userName,string password);
    }
}
