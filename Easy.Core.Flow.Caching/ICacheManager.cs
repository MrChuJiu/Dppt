using System;
using System.Collections.Generic;

namespace Easy.Core.Flow.Caching
{
    public interface ICacheManager: IDisposable
    {
        /// <summary>
        /// 获取全部缓存
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<ICache> GetAllCaches();

        /// <summary>
        /// 根据name 获取缓存
        /// 如果缓存不存在则会创建缓存
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ICache GetCache(string name);

    }
}
