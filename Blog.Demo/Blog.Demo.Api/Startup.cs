using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogDemo.Api.Extensions;
using BlogDemo.Core.Intefaces;
using BlogDemo.Infrastructure.Database;
using BlogDemo.Infrastructure.Repositories;
using BlogDemo.Infrastructure.Resources;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogDemo.Api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            //获取配置文件
            var conn1= _configuration.GetConnectionString("DefaultConnection");
           var conn2= _configuration["ConnectionStrings:DefaultConnection"];

            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlite("Data Source=BlogDemo.db");

            });
            services.AddMvc();
                //.AddFluentValidation();
            services.AddHsts(option =>
            {
                option.Preload = true;
                option.IncludeSubDomains = true;
                option.MaxAge = TimeSpan.FromDays(60);
                option.ExcludedHosts.Add("example.com");
                option.ExcludedHosts.Add("www.example.com");

            });
            //将http转为https
            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;

            });

            //注册需要的接口
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //注册AotuMapper
            services.AddAutoMapper(typeof(MappingProfile));//9.0之后需要手动指定要注入的类型
            //注册FluentValidator
            services.AddTransient<IValidator<PostResource>, PostResourceValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory
            )
        {

            app.UseHsts();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //全局错误
            app.UseMyExceptionHandler(loggerFactory);
            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
