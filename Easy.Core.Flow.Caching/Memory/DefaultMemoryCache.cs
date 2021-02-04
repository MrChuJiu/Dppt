﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;

namespace Easy.Core.Flow.Caching.Memory
{
    public class DefaultMemoryCache: CacheBase
    {
        private MemoryCache _memoryCache;
        public DefaultMemoryCache(string name) : base(name)
        {
            _memoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(new OptionsWrapper<MemoryCacheOptions>(new MemoryCacheOptions()));
        }

        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("无法将空值插入缓存");
            }

            if (absoluteExpireTime != null)
            {
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(absoluteExpireTime.Value));
            }
            else if (slidingExpireTime != null)
            {
                _memoryCache.Set(key, value, slidingExpireTime.Value);
            }
            else if (DefaultAbsoluteExpireTime != null)
            {
                _memoryCache.Set(key, value, DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value));
            }
            else
            {
                _memoryCache.Set(key, value, DefaultSlidingExpireTime);
            }
        }
        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}
