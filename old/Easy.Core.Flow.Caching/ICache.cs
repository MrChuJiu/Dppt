using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Core.Flow.Caching
{
    public  interface ICache: IDisposable
    {
        string Name { get; }
        /// <summary>
        /// 滑动过期时间
        /// </summary>
        TimeSpan DefaultSlidingExpireTime { get; set; }
        /// <summary>
        /// 默认绝对过期时间
        /// </summary>
        TimeSpan? DefaultAbsoluteExpireTime { get; set; }
        /// <summary>
        /// 从缓存中根据Key获取存储对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        object Get(string key, Func<string, object> factory);
        /// <summary>
        /// 从缓存中根据Key获取存储对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        Task<object> GetAsync(string key, Func<string, Task<object>> factory);
        /// <summary>
        /// 从缓存中根据Key获取存储对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetOrDefault(string key);
        /// <summary>
        /// 从缓存中根据Key获取存储对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<object> GetOrDefaultAsync(string key);
        /// <summary>
        /// 添加缓存项并指定过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <param name="absoluteExpireTime"></param>
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        /// <summary>
        /// 添加缓存项并指定过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="slidingExpireTime"></param>
        /// <param name="absoluteExpireTime"></param>
        /// <returns></returns>
        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);
        /// <summary>
        /// 根据key 移除指定缓存项
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 根据key 移除指定缓存项
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        void Clear();
        /// <summary>
        /// 清空所有缓存
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();
    }
}
