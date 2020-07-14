using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Dapper;
using Microsoft.Data.Sqlite;
using Working.Models.DataModel;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Working
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        public Startup(IConfiguration configuration,ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        //依赖注入的地方,比如仓储类
        public void ConfigureServices(IServiceCollection services)
        {
            /*
            _logger.LogInformation("测试数据库连接");
            var connectionString = string.Format(Configuration.GetConnectionString("DefaultConnection"),
                System.IO.Directory.GetCurrentDirectory());
            using (var con = new SqliteConnection(connectionString))
            {
                var roles = con.Query<Role>("select * from roles").ToList();
            }
            */
            //验证的注入
            services.AddAuthentication(opts=> { opts.DefaultScheme=CookieAuthenticationDefaults.AuthenticationScheme})
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,opt=> 
                {
                    opt.LoginPath = new PathString("/login");
                    opt.AccessDeniedPath = new PathString("home/error");
                    opt.LogoutPath = new PathString("/.login");
                    opt.Cookie.Path = "/";
                });
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();//添加中间件
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
