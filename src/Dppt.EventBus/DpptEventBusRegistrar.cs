using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dppt.EventBus.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Dppt.EventBus
{
    public static class DpptEventBusRegistrar
    {
        public static void AddDpptEventBus(this IServiceCollection services, List<Type> types) {


            services.AddSingleton<ILocalEventBus, LocalEventBus>();
            var localHandlers = types;/*.Where(s => typeof(ILocalEventHandler<>).IsAssignableFrom(s)).ToList();*/

            foreach (var item in localHandlers)
            {
                services.AddTransient(item);
            }

            services.Configure<LocalEventBusOptions>(options =>
            {
                options.Handlers.AddIfNotContains(localHandlers);
            });

        }
    }
}
