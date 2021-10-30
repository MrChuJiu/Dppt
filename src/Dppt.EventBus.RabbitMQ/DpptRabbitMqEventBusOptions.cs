using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class DpptRabbitMqEventBusOptions
    {
        // 交换器名称
        public string ExchangeName { get; set; }
        /// <summary>
        /// 队列名（放客户端名称）
        /// </summary>
        public string ClientName { get; set; }
    }
}
