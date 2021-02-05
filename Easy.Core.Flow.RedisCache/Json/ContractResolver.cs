using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Easy.Core.Flow.RedisCache.Json
{
    public class ContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            ModifyProperty(member, property);

            return property;
        }

        protected virtual void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                return;
            }
        }
    }
}
