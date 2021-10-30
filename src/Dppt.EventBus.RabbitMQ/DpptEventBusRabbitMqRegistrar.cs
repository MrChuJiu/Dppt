using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Dppt.EventBus.Distributed;

namespace Dppt.EventBus.RabbitMQ
{
    public static class DpptEventBusRabbitMqRegistrar
    {
        public static void AddDpptEventBusRabbitMq(this IServiceCollection services, IConfiguration configuration, List<Type> types)
        {
     
            services.AddSingleton<IRabbitMqConnections>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMqConnections>>();

                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMQ:EventBusConnection"],
                    VirtualHost = configuration["RabbitMQ:EventBusVirtualHost"],
                    DispatchConsumersAsync = true,
                    AutomaticRecoveryEnabled = true
            };

                if (!string.IsNullOrEmpty(configuration["RabbitMQ:EventBusUserName"]))
                {
                    factory.UserName = configuration["RabbitMQ:EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["RabbitMQ:EventBusPassword"]))
                {
                    factory.Password = configuration["RabbitMQ:EventBusPassword"];
                }

                return new RabbitMqConnections(factory, logger);
            });

            var distributedHandlers = types;
            foreach (var item in distributedHandlers)
            {
                services.AddSingleton(item);
            }

            services.Configure<DistributedEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(distributedHandlers);
            });

            services.Configure<DpptRabbitMqEventBusOptions>(options => {

                options.ExchangeName = configuration["RabbitMQ:EventBus:ExchangeName"];
                options.ClientName = configuration["RabbitMQ:EventBus:ClientName"];
            });

            services.AddSingleton<IDistributedEventBus, RabbitMqDistributedEventBus>();

          
        }
    }
}
