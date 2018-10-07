using Butterfly.Client.AspNetCore;
using Butterfly.Client.Tracing;
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
using System.Net.Http;

namespace Payment.Service
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
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
            //针对要保护的资源，我们需要以下配置：
            //（1）指定资源是否需要保护； ===》使用[Authorize]特性，来显式指定受保护的资源
            //（2）指定IdentityServer用来进行认证和授权跳转；
            //（3）Client携带【Token】请求资源。
            //（4）受保护的资源服务器要能够验证【Token】的正确性。
            services.AddAuthentication(Configuration["IdentityService:DefaultScheme"])
                 .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = Configuration["IdentityService:Uri"];
                     options.ApiName = "payment.service";
                     options.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
                 });

            #endregion

            #region Tracing - Butterfly

            services.AddButterfly(option =>
            {
                option.CollectorUrl = Configuration["TracingCenter:Uri"];
                option.Service = Configuration["TracingCenter:Name"];
            });

            services.AddSingleton(p => new HttpClient(p.GetService<HttpTracingHandler>()));

            #endregion
            services.AddSingleton<ILogger, ExceptionLessLogger>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
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


            // IdentityServer
            app.UseAuthentication();
            app.UseMvc();
            // exceptionless
            app.UseExceptionless(Configuration["Exceptionless:ApiKey"]);
        }
    }
}
