using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace Payment.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            app.UseMvc();
        }
    }
}
