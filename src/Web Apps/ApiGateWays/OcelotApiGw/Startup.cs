using App.Metrics;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
//using Ocelot.Provider.Consul;
//using Ocelot.Provider.Polly;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OcelotApiGw
{
    public class Startup
    {

        private readonly IConfiguration _cfg;

        public Startup(IConfiguration configuration)
        {
            _cfg = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //使用Ocelot网关组件
            //services.AddOcelot(_cfg)
            //.AddConsul();
            //利用Polly启用熔断功能
            services.AddOcelot(_cfg)
                 .AddOpenTracing(option =>
                 {
                     option.CollectorUrl = _cfg["TracingCenter:Uri"];
                     option.Service = _cfg["TracingCenter:Name"];
                 });
                 //.AddPolly();

            #region 添加swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"{_cfg["Swagger:DocName"]}", new Info
                {
                    Title = _cfg["Swagger:Title"],
                    Version = _cfg["Swagger:Version"]
                });
            });

            #endregion

            // IdentityServer
            #region IdentityServerAuthenticationOptions => need to refactor
            Action<IdentityServerAuthenticationOptions> AuthOptPayment = option =>
            {
                option.Authority = _cfg["IdentityService:Uri"];
                option.ApiName = "payment.service";
                option.RequireHttpsMetadata = Convert.ToBoolean(_cfg["IdentityService:UseHttps"]);
                option.SupportedTokens = SupportedTokens.Both;
                //option.ApiSecret = _cfg["IdentityService:ApiSecrets:paymentservice"];
            };

            Action<IdentityServerAuthenticationOptions> AuthOptOrdering = option =>
            {
                option.Authority = _cfg["IdentityService:Uri"];
                option.ApiName = "ordering.service";
                option.RequireHttpsMetadata = Convert.ToBoolean(_cfg["IdentityService:UseHttps"]);
                option.SupportedTokens = SupportedTokens.Both;
                option.ApiSecret = _cfg["IdentityService:ApiSecrets:orderingservice"];
            };

            //添加认证
            services.AddAuthentication()
                .AddIdentityServerAuthentication("PaymentServiceKey", AuthOptPayment)
                .AddIdentityServerAuthentication("OrderingServiceKey", AuthOptOrdering);

            #endregion

            #region 添加监控平台 AppMetrics服务注入

            bool isOpenMetrics = Convert.ToBoolean(_cfg["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                string database = _cfg["AppMetrics:DatabaseName"];
                string connStr = _cfg["AppMetrics:ConnectionString"];
                string app = _cfg["AppMetrics:App"];
                string env = _cfg["AppMetrics:Env"];
                string username = _cfg["AppMetrics:UserName"];
                string password = _cfg["AppMetrics:Password"];

                var uri = new Uri(connStr);
                var metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
                {
                    options.AddAppTag(app);
                    options.AddEnvTag(env);
                }).Report.ToInfluxDb(options =>
                {
                    options.InfluxDb.BaseUri = uri;
                    options.InfluxDb.Database = database;
                    options.InfluxDb.UserName = username;
                    options.InfluxDb.Password = password;
                    options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                    options.HttpPolicy.FailuresBeforeBackoff = 5;
                    options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                    options.FlushInterval = TimeSpan.FromSeconds(5);
                }).Build();

                services.AddMetrics(metrics);
                services.AddMetricsReportScheduler();
                services.AddMetricsTrackingMiddleware();
                services.AddMetricsEndpoints();

                #endregion

                services.AddMvc();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //添加控制台日志记录
            loggerFactory.AddConsole(_cfg.GetSection("Logging"));

            #region 配置swagger ui

            // get from service discovery later
            var apiList = new List<string>()
            {
                "Payment.Service",
                "Ordering.Service"
            };

            app.UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    apiList.ForEach(apiItem =>
                    {
                        options.SwaggerEndpoint($"/doc/{apiItem}/swagger.json", apiItem);
                    });
                });

            #endregion

            #region 使用AppMetrics中间件

            // AppMetrics
            bool isOpenMetrics = Convert.ToBoolean(_cfg["AppMetrics:IsOpen"]);
            if (isOpenMetrics)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();
            }

            #endregion


            //Ocelot
            app.UseOcelot().Wait();
        }
    }
}
