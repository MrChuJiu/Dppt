using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public interface IModuleManager
    {
        /// <summary>
        /// 启动模块
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        void StartModule<TModule>(IServiceCollection services)
            where TModule : IAppModule;
    }
}
