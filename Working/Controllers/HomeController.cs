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
using Working.Models.Respository;

namespace Working.Controllers
{
    //允许Manager,Leader,Employee访问Controller
    [Authorize(Roles = "Employee,Leader,Manager")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        readonly IUserRepository _userRespository;
        public HomeController(ILogger<HomeController> logger, IUserRepository userRespository)
        {
            _logger = logger;
            _userRespository = userRespository;
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
    }
}
