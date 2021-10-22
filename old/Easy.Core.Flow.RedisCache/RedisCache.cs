using System;
using System.Reflection;
using Easy.Core.Flow.Caching;
using StackExchange.Redis;

namespace Easy.Core.Flow.RedisCache
{
    public class RedisCache : CacheBase
    {
        IDatabase _database;

        IRedisCacheSerializer _serializer;

        public RedisCache(string name) : base(null)
        {

        }

        public RedisCache Initialize(IRedisCacheDatabaseProvider redisCacheDatabaseProvider, IRedisCacheSerializer redisCacheSerializer)
        {
            _database = redisCacheDatabaseProvider.GetDatabase();
            _serializer = redisCacheSerializer;
            return this;
        }

        public override void Clear()
        {
            _database.KeyDeleteWithPrefix(GetLocalizedKey("*"));
        }

        public override object GetOrDefault(string key)
        {
            var objbyte = _database.StringGet(GetLocalizedKey(key));
            return objbyte.HasValue ? Deserialize(objbyte) : null;
        }

        public override void Remove(string key)
        {
            _database.KeyDelete(GetLocalizedKey(key));
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }
            var type = value.GetType();
            _database.StringSet(
                GetLocalizedKey(key),
                Serialize(value, type),
                absoluteExpireTime ?? slidingExpireTime ?? DefaultAbsoluteExpireTime ?? DefaultSlidingExpireTime
            );
        }

        protected virtual string Serialize(object value, Type type)
        {
            return _serializer.Serialize(value, type);
        }

        protected virtual object Deserialize(RedisValue objbyte)
        {
            return _serializer.Deserialize(objbyte);
        }
        protected virtual string GetLocalizedKey(string key)
        {
            return "n:" + Name + ",c:" + key;
        }
    }
}
