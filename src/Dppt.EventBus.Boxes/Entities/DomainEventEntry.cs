using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes.Entities
{
    [Serializable]
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public object EventData { get; }

        public long EventOrder { get; }

        public DomainEventEntry(object sourceEntity, object eventData, long eventOrder)
        {
            SourceEntity = sourceEntity;
            EventData = eventData;
            EventOrder = eventOrder;
        }
    }
}
