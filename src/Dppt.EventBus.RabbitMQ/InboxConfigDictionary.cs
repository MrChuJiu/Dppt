using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class InboxConfigDictionary : Dictionary<string, InboxConfig>
    {
        public void Configure(Action<InboxConfig> configAction)
        {
            Configure("Default", configAction);
        }

        public void Configure(string outboxName, Action<InboxConfig> configAction)
        {
            var outboxConfig = this.GetOrAdd(outboxName, () => new InboxConfig(outboxName));
            configAction(outboxConfig);
        }
    }
}
