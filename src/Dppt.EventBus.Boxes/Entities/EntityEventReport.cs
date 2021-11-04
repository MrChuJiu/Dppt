using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes.Entities
{
    public class EntityEventReport
    {
        public List<DomainEventEntry> DomainEvents { get; }

        public List<DomainEventEntry> DistributedEvents { get; }

        public EntityEventReport()
        {
            DomainEvents = new List<DomainEventEntry>();
            DistributedEvents = new List<DomainEventEntry>();
        }

        public override string ToString()
        {
            return $"[{nameof(EntityEventReport)}] DomainEvents: {DomainEvents.Count}, DistributedEvents: {DistributedEvents.Count}";
        }
    }
}
