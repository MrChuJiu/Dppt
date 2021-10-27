using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class RabbitMqConnections : IRabbitMqConnections
    {
        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqConnections> _logger;
        IConnection _connection;
        bool _disposed;
        public RabbitMqConnections(IConnectionFactory connectionFactory, ILogger<RabbitMqConnections> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }


        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public void TryConnect() {

            _connection = _connectionFactory.CreateConnection();

        }


        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }


        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }

    }
}
