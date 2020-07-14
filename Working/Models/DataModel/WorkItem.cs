using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Working.Models.DataModel
{
    public class WorkItem
    {
        public int ID { get; set; }
        public string WorkContent { get; set; }
        public DateTime RecordDate { get; set; }
        public string Moms { get; set; }
        public DateTime CreateTime { get; set; }
        public int CreateUserID { get; set; }
    }
}
