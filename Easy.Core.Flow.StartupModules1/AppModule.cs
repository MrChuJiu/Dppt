using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules1
{
    public class AppModule : IAppModule
    {

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
           
        }
    }
}
