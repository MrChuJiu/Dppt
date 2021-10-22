using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public interface IEventHandlerFactory
    {
        /// <summary>
        /// Gets an event handler.
        /// </summary>
        /// <returns>The event handler</returns>
        IEventHandlerDisposeWrapper GetHandler();

        bool IsInFactories(List<IEventHandlerFactory> handlerFactories);
    }
}
