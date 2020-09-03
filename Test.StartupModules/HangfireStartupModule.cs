using Easy.Core.Flow.StartupModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.StartupModules
{
    public class HangfireStartupModule : IAppModule
    {
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("HangfireStartupModule----ConfigureServices");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Console.WriteLine("HangfireStartupModule----Configure");
        }
    }
}
