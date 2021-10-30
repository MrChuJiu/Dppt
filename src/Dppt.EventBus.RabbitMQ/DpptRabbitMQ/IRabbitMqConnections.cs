using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public interface IRabbitMqConnections : IDisposable
    {
        bool IsConnected { get; }

        void TryConnect();

        IModel CreateModel();
    }
}
