using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BlogDemo.Infrastructure.Database;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace BlogDemo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {

            #region 使用serolog,替换默认
            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.File(Path.Combine("logs", @"log.txt"),
                   rollingInterval: RollingInterval.Day,//一天创建一个
                   rollOnFileSizeLimit: true)
               .CreateLogger(); 
            #endregion

            var host=  CreateWebHostBuilder(args).Build();

            //数据初始化
            using (var scope=host.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var LoggerFactory = service.GetRequiredService<ILoggerFactory>();

                try
                {
                    var myContext = service.GetRequiredService<MyContext>();
                    MyContextSeed.SeedAsync(myContext, LoggerFactory).Wait();

                }
                catch (Exception e)
                {

                    var logger = LoggerFactory.CreateLogger<Program>();

                    logger.LogError(e, "Error occurd seeding the database");
                }


            }


            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();//会把默认替换掉
        
                //.UseStartup(typeof(StartupProduction).GetTypeInfo().Assembly.FullName);
    }
}
