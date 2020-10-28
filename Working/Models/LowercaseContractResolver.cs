using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Working.Models
{
    public class LowercaseContractResolver:Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        /// <summary>
        /// 重写ResolvePropertyName返回propertyName小写
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
