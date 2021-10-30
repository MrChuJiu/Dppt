using Dppt.EventBus.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dppt.EventBusDistrbuted.Samples.Model
{
    public class WeatherQueryHandler : IDistributedEventHandler<WeatherQueryEto>
    {
        public Task HandleEventAsync(WeatherQueryEto eventData)
        {
            Console.WriteLine(eventData.Name);
            return Task.CompletedTask;
        }
    }
}
