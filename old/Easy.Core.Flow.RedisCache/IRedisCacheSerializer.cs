using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace Easy.Core.Flow.RedisCache
{
    public interface IRedisCacheSerializer
    {
        object Deserialize(RedisValue objbyte);

        string Serialize(object value, Type type);
    }
}
