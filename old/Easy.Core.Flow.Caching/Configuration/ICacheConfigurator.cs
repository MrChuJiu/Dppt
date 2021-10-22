using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.Caching.Configuration
{
    public interface ICacheConfigurator
    {
        /// <summary>
        /// 缓存名称 如果Name为空则会设置所有缓存
        /// </summary>
        string CacheName { get; }
        /// <summary>
        /// 缓存配置
        /// </summary>
        Action<ICache> InitAction { get; }
    }
}
