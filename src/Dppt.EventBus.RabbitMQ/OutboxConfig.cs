using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class OutboxConfig
    {
        public string Name { get; }

        public Type ImplementationType { get; set; }

        public Func<Type, bool> Selector { get; set; }

        /// <summary>
        /// Used to enable/disable sending events from outbox to the message broker.
        /// Default: true.
        /// </summary>
        public bool IsSendingEnabled { get; set; } = true;

        public OutboxConfig(string name)
        {
            Name = name;
        }
    }
}
