using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Easy.Core.Flow.YoYoMoocDocker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {



                    webBuilder.UseStartup<Startup>()
                    .ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.IPv6Any, 443, listenOptions =>
                        {
                            Console.WriteLine("pfx£º" + Path.Combine(AppContext.BaseDirectory, "socialnetwork.pfx"));
                            listenOptions.UseHttps(Path.Combine(AppContext.BaseDirectory, "socialnetwork.pfx"), "handong123");
                        });

                    });
                });
    }
}
