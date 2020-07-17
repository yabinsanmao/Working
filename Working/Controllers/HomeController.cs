﻿using System;
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

namespace Working.Controllers
{
    //允许Manager,Leader,Employee访问Controller
    [Authorize(Roles ="Manager,Leader,Employee")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
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
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]//允许匿名用户访问，任何用户都可以访问Login页面
        [HttpPost("login")]
        public IActionResult Login(string userName, string passWord)
        {
            if (userName == "aa" && passWord == "bb")
            {
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Role,"Leader"),
                    new Claim(ClaimTypes.Name,"桂素伟"),
                    new Claim(ClaimTypes.Sid,"1")
                };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(new ClaimsIdentity(claims)));
                return new RedirectResult("/");
            }
            else
            {
                ViewBag.error = "用户名或密码错误";
                return View();
            }
        }
    }
}
