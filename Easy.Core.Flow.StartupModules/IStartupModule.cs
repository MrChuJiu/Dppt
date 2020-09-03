using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Easy.Core.Flow.StartupModules
{
    public interface IStartupModule
    {
        void ConfigureServices(IServiceCollection services, ConfigureServicesContext context);

        void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context);
    }
}
