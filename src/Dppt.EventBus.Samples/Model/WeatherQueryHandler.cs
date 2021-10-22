using Dppt.EventBus.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dppt.EventBus.Samples.Model
{
    public class WeatherQueryHandler : ILocalEventHandler<WeatherQueryEto>
    {
        public Task HandleEventAsync(WeatherQueryEto eventData)
        {
            Console.WriteLine(eventData.Name);
            return Task.CompletedTask;
        }
    }
}
