﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class ExchangeDeclareConfiguration
    {
        public string ExchangeName { get; }

        public string Type { get; }

        public bool Durable { get; set; }

        public bool AutoDelete { get; set; }

        public IDictionary<string, object> Arguments { get; }

        public ExchangeDeclareConfiguration(
            string exchangeName,
            string type,
            bool durable = false,
            bool autoDelete = false)
        {
            ExchangeName = exchangeName;
            Type = type;
            Durable = durable;
            AutoDelete = autoDelete;
            Arguments = new Dictionary<string, object>();
        }
    }
}
