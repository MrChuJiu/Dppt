using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public class EventNameAttribute : Attribute, IEventNameProvider
    {
        public virtual string Name { get; }

        public EventNameAttribute([NotNull] string name)
        {
            Name = name;
        }

        public static string GetNameOrDefault<TEvent>()
        {
            return GetNameOrDefault(typeof(TEvent));
        }

        public static string GetNameOrDefault([NotNull] Type eventType)
        {
            return eventType
                       .GetCustomAttributes(true)
                       .OfType<IEventNameProvider>()
                       .FirstOrDefault()
                       ?.GetName(eventType)
                   ?? eventType.FullName;
        }


        public string GetName(Type eventType)
        {
            return Name;
        }
    }
}
