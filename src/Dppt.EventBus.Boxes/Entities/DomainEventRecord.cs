using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes.Entities
{
    public class DomainEventRecord
    {
        public object EventData { get; }

        public long EventOrder { get; }

        public DomainEventRecord(object eventData, long eventOrder)
        {
            EventData = eventData;
            EventOrder = eventOrder;
        }
    }
}
