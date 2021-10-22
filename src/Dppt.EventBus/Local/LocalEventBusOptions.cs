using Dppt.Core.Dppt.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Local
{
    public class LocalEventBusOptions
    {
        public ITypeList<IEventHandler> Handlers { get; }

        public LocalEventBusOptions()
        {
            Handlers = new TypeList<IEventHandler>();
        }
    }
}
