using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes.Entities
{
    public interface IGeneratesDomainEvents
    {
        IEnumerable<DomainEventRecord> GetLocalEvents();

        IEnumerable<DomainEventRecord> GetDistributedEvents();

        void ClearLocalEvents();

        void ClearDistributedEvents();
    }
}
