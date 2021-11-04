using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes.Entities
{
    public static class EventOrderGenerator
    {
        private static long _lastOrder;

        public static long GetNext()
        {
            return Interlocked.Increment(ref _lastOrder);
        }
    }
}
