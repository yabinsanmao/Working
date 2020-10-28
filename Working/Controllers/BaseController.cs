using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Working.Models;

namespace Working.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 返回Json
        /// </summary>
        /// <param name="backResult">处理结果</param>
        /// <param name="message">消息</param>
        /// <param name="data">返回数据</param>
        /// <param name="dateFormat">日期格式</param>
        /// <returns></returns>
        protected JsonResult ToJson(BackResult backResult, string message = "", dynamic data = null, string dateFormat = "yyyy年MM月dd日")
        {
            return new JsonResult(new { result = (int)backResult, message = message, data = data }, new Newtonsoft.Json.JsonSerializerSettings() { ContractResolver = new LowercaseContractResolver(), DateFormatString = dateFormat });
        }
    }
}
