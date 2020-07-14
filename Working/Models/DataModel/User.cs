using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Working.Models.DataModel
{
    public class User
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Name { get; set; }
        public int RoleID { get; set; }
        public int DepartmentID { get; set; }
    }
}
