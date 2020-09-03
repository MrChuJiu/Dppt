using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules.Internal
{
    /// <summary>
    /// 提供内嵌的中间件配置
    /// </summary>
    public class InlineMiddlewareConfiguration : IStartupModule
    {
        private readonly Action<IApplicationBuilder, ConfigureMiddlewareContext> _action;
        public InlineMiddlewareConfiguration(Action<IApplicationBuilder, ConfigureMiddlewareContext> action)
        {
            _action = action;
        }
        public void Configure(IApplicationBuilder app, ConfigureMiddlewareContext context)
        {
            _action(app, context);
        }

        public void ConfigureServices(IServiceCollection services, ConfigureServicesContext context)
        {
            
        }
    }
}
