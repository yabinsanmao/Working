using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Working.Models;
using Working.Models.DataModel;
using Working.Models.Respository;

namespace Working.Controllers
{
    //允许Manager,Leader,Employee访问Controller
    [Authorize(Roles = "Employee,Leader,Manager")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// 用户仓储
        /// </summary>
        readonly IUserRepository _userRespository;
        /// <summary>
        /// 部门仓储
        /// </summary>
        readonly IDepartmentRepository _departmentRepository;
        public HomeController(ILogger<HomeController> logger, IUserRepository userRespository, IDepartmentRepository departmentRepository)
        {
            _logger = logger;
            _userRespository = userRespository;
            _departmentRepository = departmentRepository;
        }
        //[Authorize]
        public IActionResult Index()
        {
            _logger.LogInformation("Index page load successfully!");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            _logger.LogDebug("this is about page!thanks");
            return View();
        }
        [Authorize(Roles = "Manager")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [AllowAnonymous]//允许匿名用户访问，任何用户都可以访问Login页面
        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                ViewBag.returnUrl = returnUrl;
            }
            return View();
        }
        [AllowAnonymous]//允许匿名用户访问，任何用户都可以访问Login页面
        [HttpPost("login")]
        public IActionResult Login(string userName, string passWord, string returnUrl)
        {
            try
            {
                var user = _userRespository.Login(userName, passWord);
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role,user.RoleName),
                    new Claim(ClaimTypes.Name,user.Name),
                    new Claim(ClaimTypes.Sid,user.ID.ToString())
                };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                return new RedirectResult(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
            }
            catch (Exception exc)
            {
                ViewBag.error = exc.Message;
                return View();
            }
        }
        [HttpGet("departments")]
        public IActionResult Departments()
        {
            return View();
        }
        /// <summary>
        /// 获取所有带父级部门的部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getallpdepartment")]
        public IActionResult GetAllPDepartments()
        {
            try
            {
                var list = _departmentRepository.GetAllPDepartment();

                return ToJson(BackResult.Success, data: list);
                //return new JsonResult(new { result = 1, data = list, message = "查询成功" });
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 获取部门
        /// </summary>
        /// <returns></returns>
        [HttpGet("getalldepartment")]
        public IActionResult GetAllDepartments()
        {
            try
            {
                var list = _departmentRepository.GetAllDepartment();

                return ToJson(BackResult.Success, data: list);
                //return new JsonResult(new { result = 1, data = list, message = "查询成功" });
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost("adddepartment")]
        public IActionResult AddDepartment(Department department)
        {
            try {
                var result = _departmentRepository.AddDepartment(department);
                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "添加成功":"添加失败");
            }
            catch(Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 编辑部门
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPut("editdepartment")]
        public IActionResult EditDepartment(Department department)
        {
            try
            {
                var result = _departmentRepository.EditDepartment(department);

                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "修改成功" : "修改失败");
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="departmentID"></param>
        /// <returns></returns>
        [HttpDelete("removedepartment")]
        public IActionResult RemoveDepartment(int departmentID)
        {
            try
            {
                var result = _departmentRepository.RemoveDepartment(departmentID);

                return ToJson(result ? BackResult.Success : BackResult.Fail, message: result ? "删除成功" : "删除失败");
            }
            catch (Exception exc)
            {
                return ToJson(BackResult.Exception, message: exc.Message);
            }
        }
    }
}
