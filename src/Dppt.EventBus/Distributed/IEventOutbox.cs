using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public interface IEventOutbox
    {
        Task EnqueueAsync(OutgoingEventInfo outgoingEvent);
    }
}
