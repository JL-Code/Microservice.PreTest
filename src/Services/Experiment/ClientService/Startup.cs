﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClientService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region IdentityServer4

            services.AddMvcCore().AddAuthorization().AddJsonFormatters();
            services.AddAuthentication(Configuration["Identity:Scheme"])
                .AddIdentityServerAuthentication(options =>
                {
                    options.RequireHttpsMetadata = false; // for dev env
                    options.Authority = $"http://{Configuration["Identity:Address"]}:{Configuration["Identity:Port"]}";
                    options.ApiName = Configuration["Service:Name"]; // match with configuration in IdentityServer
                });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // authentication
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
