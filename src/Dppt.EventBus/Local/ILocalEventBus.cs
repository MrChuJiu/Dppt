using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Local
{
    public interface ILocalEventBus : IEventBus
    {
        Task PublishAsync(Type eventType, object eventData);
    }
}
