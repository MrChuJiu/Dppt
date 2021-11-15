using Dppt.Core.Dppt.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public class DistributedEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public OutboxConfigDictionary Outboxes { get; }

        public InboxConfigDictionary Inboxes { get; }


        public DistributedEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}
