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
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(configuration["EventBusUserName"]))
                {
                    factory.UserName = configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(configuration["EventBusPassword"]))
                {
                    factory.Password = configuration["EventBusPassword"];
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

            services.Configure<DpptRabbitMqEventBusOptions>(options =>
            {
                options.ExchangeName = "";
            });

            services.AddSingleton<IDistributedEventBus, RabbitMqDistributedEventBus>();

        }
    }
}
