using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules1
{
    public interface IStartupModuleRunner
    {
        void ConfigureServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostingEnvironment);


        void Configure(IApplicationBuilder app, IConfiguration configuration, IWebHostEnvironment hostingEnvironment);
    }
}
