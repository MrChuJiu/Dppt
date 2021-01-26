using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.DependencyInjection
{
    public static class DependencyInjectionServiceConventionalRegistrationExtensions
    {

        public static IServiceCollection AddAssembly(this IServiceCollection services, Assembly assembly)
        {
            // 这里其实可以通过依赖注入 或者 接口实现替换的方式来自定义AddType的实现
            new DefaultConventionalRegistrar().AddAssembly(services, assembly);
            return services;
        }
    }
}
