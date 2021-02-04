using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.Caching.Configuration
{
    /// <summary>
    /// 用于配置缓存系统
    /// </summary>
    public interface  ICachingConfiguration
    {
        /// <summary>
        /// 所有注册程序列表
        /// </summary>
        IReadOnlyList<ICacheConfigurator> Configurators { get; }
        /// <summary>
        /// 用于设置所有缓存
        /// </summary>
        /// <param name="initAction"></param>
        void ConfigureAll(Action<ICache> initAction);
        /// <summary>
        /// 用于设置特定的缓存
        /// </summary>
        /// <param name="cacheName"></param>
        /// <param name="initAction"></param>
        void Configure(string cacheName, Action<ICache> initAction);
    }
}
