using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public class InboxConfig
    {
        public string Name { get; }

        public Type ImplementationType { get; set; }

        public Func<Type, bool> EventSelector { get; set; }

        public Func<Type, bool> HandlerSelector { get; set; }

        /// <summary>
        /// Used to enable/disable processing incoming events.
        /// Default: true.
        /// </summary>
        public bool IsProcessingEnabled { get; set; } = true;

        public InboxConfig( string name)
        {
            Name = name;
        }
    }
}
