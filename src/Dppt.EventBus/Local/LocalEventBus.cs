using Dppt.Core.Dppt.Collections;
using Dppt.Core.Dppt.Threading;
using Dppt.EventBus.Local;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dppt.EventBus
{
    public class LocalEventBus : ILocalEventBus
    {
        protected IServiceScopeFactory ServiceScopeFactory { get; }

        protected LocalEventBusOptions Options { get; }

        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }


        public LocalEventBus(
            IOptions<LocalEventBusOptions> options,
            IServiceScopeFactory serviceScopeFactory)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();

            SubscribeHandlers(Options.Handlers);
        }

        public async Task PublishAsync<TEvent>(TEvent eventData)
        {
            await TriggerHandlersAsync(typeof(TEvent), eventData);
        }


        protected virtual async Task TriggerHandlersAsync(Type eventType, object eventData)
        {

            foreach (var handlerFactories in GetHandlerFactories(eventType))
            {
                foreach (var handlerFactory in handlerFactories.EventHandlerFactories)
                {
                    await TriggerHandlerAsync(handlerFactory, handlerFactories.EventType, eventData);
                }
            }
        }

        protected virtual async Task TriggerHandlerAsync(IEventHandlerFactory asyncHandlerFactory, Type eventType, object eventData)
        {
            using (var eventHandlerWrapper = asyncHandlerFactory.GetHandler())
            {
                var handlerType = eventHandlerWrapper.EventHandler.GetType();

                var method = typeof(ILocalEventHandler<>)
                            .MakeGenericType(eventType)
                            .GetMethod(
                                nameof(ILocalEventHandler<object>.HandleEventAsync),
                                new[] { eventType }
                            );

                await ((Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData }));
            }
        }

        protected IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
            }

            return handlerFactoryList.ToArray();
        }

        private static bool ShouldTriggerEventForHandler(Type targetEventType, Type handlerEventType)
        {
            //Should trigger same type
            if (handlerEventType == targetEventType)
            {
                return true;
            }

            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }


        protected virtual void SubscribeHandlers(ITypeList<IEventHandler> handlers) {

            foreach (var handler in handlers)
            {
                var interfaces = handler.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (!typeof(IEventHandler).GetTypeInfo().IsAssignableFrom(@interface))
                    {
                        continue;
                    }

                    var genericArgs = @interface.GetGenericArguments();
                    if (genericArgs.Length == 1)
                    {
                        Subscribe(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                    }
                }
            }

        }

        public IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType)
                .Locking(factories =>
                {
                    if (!factory.IsInFactories(factories))
                    {
                        factories.Add(factory);
                    }
                }
                );

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(eventType, (type) => new List<IEventHandlerFactory>());
        }

        public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }
    }


    public class EventTypeWithEventHandlerFactories
    {
        public Type EventType { get; }

        public List<IEventHandlerFactory> EventHandlerFactories { get; }

        public EventTypeWithEventHandlerFactories(Type eventType, List<IEventHandlerFactory> eventHandlerFactories)
        {
            EventType = eventType;
            EventHandlerFactories = eventHandlerFactories;
        }
    }
}
