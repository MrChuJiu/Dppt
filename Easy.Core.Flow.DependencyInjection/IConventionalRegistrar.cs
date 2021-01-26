using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.DependencyInjection
{
    public interface IConventionalRegistrar
    {

        void AddAssembly(IServiceCollection services, Assembly assembly);

        void AddTypes(IServiceCollection services, params Type[] types);

        void AddType(IServiceCollection services, Type type);
    }
}
