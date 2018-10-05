using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Identity.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 为了要把IdentityServer注册到容器中，需要对其进行配置，而这个配置中要包含三个信息：
            //（1）哪些API可以使用这个AuthorizationServer
            //（2）哪些Client可以使用这个AuthorizationServer
            //（3）哪些User可以被这个AuthrizationServer识别并授权
            InMemoryConfiguration.Configuration = Configuration;
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                    .AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                    .AddInMemoryClients(InMemoryConfiguration.GetClients())
                    .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources());


            // for QuickStart-UI
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //添加 IdentityServer4服务支持
            app.UseIdentityServer();

            #region for QuickStart-UI

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

            #endregion

        }
    }
}
