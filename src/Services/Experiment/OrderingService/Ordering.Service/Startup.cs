using Butterfly.Client.AspNetCore;
using Core.Infrastructure;
using Exceptionless;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Ordering.Service
{
    /// <summary>
    /// 程序启动的时候调用
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 相关配置信息接口
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 配置服务
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 添加swagger文档功能

            //配置swagger选项
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(Configuration["Service:DocName"], new Info
                {
                    Title = Configuration["Service:Title"],
                    Version = Configuration["Service:Version"],
                    Description = Configuration["Service:Description"],
                    Contact = new Contact
                    {
                        Name = Configuration["Service:Contact:Name"],
                        Email = Configuration["Service:Contact:Email"]
                    }
                });

                //添加xml注释
                var filePath = Path.Combine(AppContext.BaseDirectory, $"{Configuration["Service:DocName"]}.xml");
                options.IncludeXmlComments(filePath);
            });

            #endregion

            #region 添加IdentityServer4 验证功能

            services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = Configuration["IdentityService:Uri"];
                options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
            });

            #endregion

            // Tracing - Butterfly
            services.AddButterfly(option =>
            {
                option.CollectorUrl = Configuration["TracingCenter:Uri"];
                option.Service = Configuration["TracingCenter:Name"];
            });

            services.AddSingleton<ILogger, ExceptionLessLogger>();
        }

        /// <summary>
        /// 配置中间件
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 配置swagger ui

            //使用swagger
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "doc/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/doc/{Configuration["Service:DocName"]}/swagger.json",
                  $"{Configuration["Service:Name"]} {Configuration["Service:Version"]}");
            });

            #endregion

            app.UseMvc();

            // IdentityServer
            app.UseAuthentication();

            // exceptionless
            app.UseExceptionless(Configuration["Exceptionless:ApiKey"]);
        }
    }
}
