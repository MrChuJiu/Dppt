using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.Distributed
{
    public class OutgoingEventInfo 
    {
        public Guid Id { get; }

        public string EventName { get; }

        public byte[] EventData { get; }

        public DateTime CreationTime { get; }

        public OutgoingEventInfo(
            Guid id,
            string eventName,
            byte[] eventData,
            DateTime creationTime)
        {
            Id = id;
            EventName = eventName;
            EventData = eventData;
            CreationTime = creationTime;
        }
    }
}
