using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public interface IDistributedEventBus : IEventBus
    {


        Task PublishAsync(
         Type eventType,
         object eventData,
         bool onUnitOfWorkComplete = true,
         bool useOutbox = true);
    }
}
