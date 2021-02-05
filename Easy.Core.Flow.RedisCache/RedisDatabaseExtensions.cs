using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RedisCache
{
    internal static class RedisDatabaseExtensions
    {
        public static void KeyDeleteWithPrefix(this IDatabase database, string prefix)
        {
            if (database == null)
            {
                throw new ArgumentException("Database cannot be null", nameof(database));
            }

            if (string.IsNullOrWhiteSpace(prefix))
            {
                throw new ArgumentException("Prefix cannot be empty", nameof(database));
            }

            database.ScriptEvaluate(@"
                local keys = redis.call('keys', ARGV[1]) 
                for i=1,#keys,5000 do 
                redis.call('del', unpack(keys, i, math.min(i+4999, #keys)))
                end", values: new RedisValue[] { prefix });
        }
    }
}
