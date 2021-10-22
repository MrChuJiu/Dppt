using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Easy.Core.Flow.RedisCache.Json;

namespace Easy.Core.Flow.RedisCache
{
    class DefaultRedisCacheSerializer : IRedisCacheSerializer
    {
        public object Deserialize(RedisValue objbyte)
        {
            return JsonSerializationHelper.DeserializeWithType(objbyte);
        }

        public string Serialize(object value, Type type)
        {
            return JsonSerializationHelper.SerializeWithType(value, type);
        }
    }
}
