using Dppt.EventBus.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dppt.EventBusDistrbuted.Samples.Model
{
    [EventName("Test_Weather_Query")]
    public class WeatherQueryEto
    {
        public string Name { get; set; }
    }
}
