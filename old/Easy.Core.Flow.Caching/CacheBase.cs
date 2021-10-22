using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace Easy.Core.Flow.Caching
{
    /// <summary>
    /// 缓存基类
    /// </summary>
    public abstract class CacheBase : ICache
    {
        public string Name { get; protected set; }

        public TimeSpan DefaultSlidingExpireTime { get; set; }

        public TimeSpan? DefaultAbsoluteExpireTime { get; set; }


        protected readonly object SyncObj = new object();

        private readonly AsyncLock _asyncLock = new AsyncLock();

        protected CacheBase(string name)
        {
            Name = name;
            DefaultSlidingExpireTime = TimeSpan.FromHours(1);
        }



        public abstract void Clear();

        public virtual Task ClearAsync()
        {
            Clear();
            return Task.FromResult(0);
        }

        public virtual void Dispose()
        {

        }

        public virtual object Get(string key, Func<string, object> factory)
        {
            object item = null;
           
            // 防止缓存击穿，先进行数据查找
            try
            {
                item = GetOrDefault(key);
            }
            catch (Exception ex)
            {
                return new Exception(ex.Message);
            }
            // 如果数据为空，加互斥锁，让其他线城进入等待
            if (item == null)
            {
                lock (SyncObj)
                {
                    try
                    {
                        item = GetOrDefault(key);
                    }
                    catch (Exception ex)
                    {
                        return new Exception(ex.Message);
                    }
                    if (item == null)
                    {
                        // 如果为空获取委托设置的值
                        item = factory(key);

                        if (item == null)
                        {
                            return null;
                        }
                        // 重新写入缓存中
                        try
                        {
                            Set(key, item);
                        }
                        catch (Exception ex)
                        {
                            return new Exception(ex.Message);
                        }
                    }
                }
            }

            return item;
        }

        public virtual async Task<object> GetAsync(string key, Func<string, Task<object>> factory)
        {
            object item = null;

            try
            {
                item = await GetOrDefaultAsync(key);
            }
            catch (Exception ex)
            {
               
            }

            if (item == null)
            {
                using (await _asyncLock.LockAsync())
                {
                    try
                    {
                        item = await GetOrDefaultAsync(key);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    if (item == null)
                    {
                        item = await factory(key);

                        if (item == null)
                        {
                            return null;
                        }
                        try
                        {
                            await SetAsync(key, item);
                        }
                        catch (Exception ex)
                        {
                           
                        }
                    }
                }
            }


            return item;

        }

        public abstract object GetOrDefault(string key);


        public virtual Task<object> GetOrDefaultAsync(string key)
        {
            return Task.FromResult(GetOrDefault(key));
        }

        public abstract void Remove(string key);

        public virtual Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.FromResult(0);
        }


        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);


        public virtual Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            Set(key, value, slidingExpireTime);
            return Task.FromResult(0);
        }
    }
}
