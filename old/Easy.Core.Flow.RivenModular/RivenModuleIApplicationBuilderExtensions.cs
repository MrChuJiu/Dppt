using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public static class RivenModuleIApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用RivenModule
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceProvider UseRivenModule(this IServiceProvider serviceProvider)
        {
            var moduleManager = serviceProvider.GetService<IModuleManager>();

            return moduleManager.ApplicationInitialization(serviceProvider);
        }
    }
}
