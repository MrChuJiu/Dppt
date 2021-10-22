using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public class IocEventHandlerFactory : IEventHandlerFactory, IDisposable
    {
        public Type HandlerType { get; }

        protected IServiceScopeFactory ScopeFactory { get; }

        public IocEventHandlerFactory(IServiceScopeFactory scopeFactory, Type handlerType)
        {
            ScopeFactory = scopeFactory;
            HandlerType = handlerType;
        }


        public IEventHandlerDisposeWrapper GetHandler()
        {
            var scope = ScopeFactory.CreateScope();
            return new EventHandlerDisposeWrapper(
                (IEventHandler)scope.ServiceProvider.GetRequiredService(HandlerType),
                () => scope.Dispose()
            );
        }

        public bool IsInFactories(List<IEventHandlerFactory> handlerFactories)
        {
            return handlerFactories
                .OfType<IocEventHandlerFactory>()
                .Any(f => f.HandlerType == HandlerType);
        }

        public void Dispose()
        {

        }
    }
}
