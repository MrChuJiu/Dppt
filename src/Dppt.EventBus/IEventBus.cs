using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent eventData);

        void Unsubscribe(Type eventType, IEventHandlerFactory factory);
    }
}
