using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO;

namespace OcelotApiGw
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CreateWebHostBuilder(args).Build().Run();
            BuildWebHost(args).Run();
            //new WebHostBuilder()
            //.UseKestrel()
            //.UseContentRoot(Directory.GetCurrentDirectory())
            //.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    config
            //        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            //        .AddJsonFile("appsettings.json", true, true)
            //        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            //        .AddJsonFile(Path.Combine("configuration", "configuration.json"))
            //        .AddEnvironmentVariables();
            //})
            //.ConfigureServices(s => {
            //    s.AddOcelot();
            //})
            //.ConfigureLogging((hostingContext, logging) =>
            //{
            //    //add your logging
            //    logging.AddDebug();
            //})
            //.UseIISIntegration()
            //.Configure(app =>
            //{
            //    app.UseOcelot().Wait();
            //})
            //.Build()
            //.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args)
        {
            IWebHostBuilder builder = WebHost.CreateDefaultBuilder(args);
            //???
            builder.ConfigureServices(s => s.AddSingleton(builder))
                .ConfigureAppConfiguration(ic => ic.AddJsonFile(Path.Combine("configuration", "configuration.json"), false, true))
                .UseStartup<Startup>();
            IWebHost host = builder.Build();
            return host;
        }
    }
}
