using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public class EventHandlerDisposeWrapper : IEventHandlerDisposeWrapper
    {
        public IEventHandler EventHandler { get; }

        private readonly Action _disposeAction;

        public EventHandlerDisposeWrapper(IEventHandler eventHandler, Action disposeAction = null)
        {
            _disposeAction = disposeAction;
            EventHandler = eventHandler;
        }

        public void Dispose()
        {
            _disposeAction?.Invoke();
        }
    }
}
