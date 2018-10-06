using Identity.Service.Application.Services;
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
            // 作为一个独立的Identity Server，它必须知道哪些资源需要保护，必须知道哪些客户端能够允许访问，这是配置的基础。
            // 为了要把IdentityServer注册到容器中，需要对其进行配置，而这个配置中要包含三个信息：
            //（1）哪些API可以使用这个AuthorizationServer
            //（2）哪些Client可以使用这个AuthorizationServer
            //（3）哪些User可以被这个AuthrizationServer识别并授权
            //针对要保护的资源，我们需要以下配置：
            //（1）指定资源是否需要保护； ===》使用[Authorize]特性，来显式指定受保护的资源
            //（2）指定IdentityServer用来进行认证和授权跳转；
            //（3）Client携带【Token】请求资源。
            //（4）受保护的资源服务器要能够验证【Token】的正确性。
            InMemoryConfiguration.Configuration = Configuration;
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())//身份资源
                    //.AddTestUsers(InMemoryConfiguration.GetUsers().ToList())
                    .AddInMemoryClients(InMemoryConfiguration.GetClients())//预置允许验证的Client
                    .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())//API资源
                    //添加自定义验证逻辑
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddProfileService<ProfileService>();
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
