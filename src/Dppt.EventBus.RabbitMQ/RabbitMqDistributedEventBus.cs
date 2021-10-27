using Dppt.Core.Dppt;
using Dppt.Core.Dppt.Collections;
using Dppt.EventBus.Distributed;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dppt.EventBus.RabbitMQ
{
    public class RabbitMqDistributedEventBus : IDistributedEventBus
    {
        protected DpptRabbitMqEventBusOptions RabbitMqEventBusOptions { get; }
        public readonly IRabbitMqConnections _rabbitMqConnections;

        protected IServiceScopeFactory ServiceScopeFactory { get; }
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        protected ConcurrentDictionary<string, Type> EventTypes { get; }

        protected ExchangeDeclareConfiguration Exchange { get; private set; }
        protected QueueDeclareConfiguration Queue { get; private set; }
        protected IModel Channel { get; private set; }


        //private IModel _consumerChannel;

        public RabbitMqDistributedEventBus(IRabbitMqConnections rabbitMqConnections, DpptRabbitMqEventBusOptions rabbitMqEventBusOptions, IServiceScopeFactory serviceScopeFactory)
        {
            _rabbitMqConnections = rabbitMqConnections;
            RabbitMqEventBusOptions = rabbitMqEventBusOptions;
            ServiceScopeFactory = serviceScopeFactory;
            //_consumerChannel = CreateConsumerChannel();
            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
        }

        public Task PublishAsync<TEvent>(TEvent eventData)
        {
            var eventName = EventNameAttribute.GetNameOrDefault(typeof(TEvent));
            var body = JsonSerializer.Serialize(eventData);
            return PublishAsync(eventName, body, null, null);
        }


        public Task PublishAsync(string eventName, string body, IBasicProperties properties, Dictionary<string, object> headersArguments = null, Guid? eventId = null)
        {

            if (!_rabbitMqConnections.IsConnected)
            {
                _rabbitMqConnections.TryConnect();
            }
            using (var channel = _rabbitMqConnections.CreateModel())
            {
                // durable 设置队列持久化  
                channel.ExchangeDeclare(RabbitMqEventBusOptions.ExchangeName, "direct", durable: true);

                if (properties == null)
                {
                    properties = channel.CreateBasicProperties();
                    // 设置消息持久化
                    properties.DeliveryMode = 2;
                }

                if (properties.MessageId.IsNullOrEmpty())
                {
                    // 消息的唯一性标识
                    properties.MessageId = (eventId ?? Guid.NewGuid()).ToString("N");
                }

                SetEventMessageHeaders(properties, headersArguments);

                channel.BasicPublish(
                   exchange: RabbitMqEventBusOptions.ExchangeName,
                   routingKey: eventName,
                   mandatory: true,
                   basicProperties: properties,
                   body: Encoding.UTF8.GetBytes(body)
               );

            }

            return Task.CompletedTask;
        }

        private void SetEventMessageHeaders(IBasicProperties properties, Dictionary<string, object> headersArguments)
        {
            if (headersArguments == null)
            {
                return;
            }

            properties.Headers ??= new Dictionary<string, object>();

            foreach (var header in headersArguments)
            {
                properties.Headers[header.Key] = header.Value;
            }
        }

        //private IModel CreateConsumerChannel()
        //{
        //    if (!_rabbitMqConnections.IsConnected)
        //    {
        //        _rabbitMqConnections.TryConnect();
        //    }

        //    var channel = _rabbitMqConnections.CreateModel();

        //    channel.ExchangeDeclare(exchange: RabbitMqEventBusOptions.ExchangeName,
        //                            type: "direct");


        //    channel.QueueDeclare(queue: "",
        //                         durable: true,
        //                         exclusive: false,
        //                         autoDelete: false,
        //                         arguments: null);

        //    channel.CallbackException += (sender, ea) =>
        //    {
        //        _consumerChannel.Dispose();
        //        _consumerChannel = CreateConsumerChannel();
        //        StartBasicConsume();
        //    };

        //    return channel;
        //}

        //private void StartBasicConsume()
        //{
        //    if (_consumerChannel != null)
        //    {
        //        var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

        //        consumer.Received += Consumer_Received;

        //        _consumerChannel.BasicConsume(
        //            queue: "",
        //            autoAck: false,
        //            consumer: consumer);
        //    }
        //}

        //private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        //{
        //    var eventName = eventArgs.RoutingKey;
        //    var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

        //    try
        //    {
        //        if (message.ToLowerInvariant().Contains("throw-fake-exception"))
        //        {
        //            throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
        //        }

        //        await ProcessEvent(eventName, message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"----- ERROR Processing message \"{ex.Message}\"");
        //    }
        //    // 就算出现消息，也要发回消息确认（可以采用死信队列）
        //    _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        //}

        //private async Task ProcessEvent(string eventName, string message)
        //{
        //    if (_subsManager.HasSubscriptionsForEvent(eventName))
        //    {
        //        using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
        //        {
        //            var subscriptions = _subsManager.GetHandlersForEvent(eventName);
        //            foreach (var subscription in subscriptions)
        //            {
        //                if (subscription.IsDynamic)
        //                {
        //                    var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
        //                    if (handler == null) continue;
        //                    using dynamic eventData = JsonDocument.Parse(message);
        //                    await Task.Yield();
        //                    await handler.Handle(eventData);
        //                }
        //                else
        //                {
        //                    var handler = scope.ResolveOptional(subscription.HandlerType);
        //                    if (handler == null) continue;
        //                    var eventType = _subsManager.GetEventTypeByName(eventName);
        //                    var integrationEvent = JsonSerializer.Deserialize(message, eventType, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        //                    var concreteType = typeof(IDistributedEventHandler<>).MakeGenericType(eventType);

        //                    await Task.Yield();
        //                    await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
        //    }
        //}


        public void Initialize(ExchangeDeclareConfiguration exchange, QueueDeclareConfiguration queue)
        {

            Exchange = exchange;
            Queue = queue;

            // 启动一个消费者
            if (!_rabbitMqConnections.IsConnected)
            {
                _rabbitMqConnections.TryConnect();
            }
            using (Channel = _rabbitMqConnections.CreateModel())
            {

                Channel.ExchangeDeclare(
                  exchange: Exchange.ExchangeName,
                  type: Exchange.Type,
                  durable: Exchange.Durable,
                  autoDelete: Exchange.AutoDelete,
                  arguments: Exchange.Arguments
              );


                Channel.QueueDeclare(
                   queue: Queue.QueueName,
                   durable: Queue.Durable,
                   exclusive: Queue.Exclusive,
                   autoDelete: Queue.AutoDelete,
                   arguments: Queue.Arguments
               );

                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += HandleIncomingMessageAsync;

                Channel.BasicConsume(
                    queue: Queue.QueueName,
                    autoAck: false,
                    consumer: consumer
                );

            }
        }


        protected virtual async Task HandleIncomingMessageAsync(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                Channel.BasicAck(basicDeliverEventArgs.DeliveryTag, multiple: false);
            }
            catch (Exception ex)
            {
                try
                {
                    Channel.BasicNack(
                        basicDeliverEventArgs.DeliveryTag,
                        multiple: false,
                        requeue: true
                    );
                }
                catch { }
            }
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

                var method = typeof(IDistributedEventHandler<>)
                                .MakeGenericType(eventType)
                                .GetMethod(
                                    nameof(IDistributedEventHandler<object>.HandleEventAsync),
                                    new[] { eventType }
                                );

                await ((Task)method.Invoke(eventHandlerWrapper.EventHandler, new[] { eventData }));
            }
        }

        protected IEnumerable<EventTypeWithEventHandlerFactories> GetHandlerFactories(Type eventType)
        {
            var handlerFactoryList = new List<EventTypeWithEventHandlerFactories>();

            foreach (var handlerFactory in
                HandlerFactories.Where(hf => ShouldTriggerEventForHandler(eventType, hf.Key)))
            {
                handlerFactoryList.Add(
                    new EventTypeWithEventHandlerFactories(handlerFactory.Key, handlerFactory.Value));
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

            //TODO: Support inheritance? But it does not support on subscription to RabbitMq!
            //Should trigger for inherited types
            if (handlerEventType.IsAssignableFrom(targetEventType))
            {
                return true;
            }

            return false;
        }

        protected virtual void SubscribeHandlers(ITypeList<IEventHandler> handlers)
        {
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

        public override IDisposable Subscribe(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);

            if (factory.IsInFactories(handlerFactories))
            {
                return NullDisposable.Instance;
            }

            handlerFactories.Add(factory);

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                Consumer.BindAsync(EventNameAttribute.GetNameOrDefault(eventType));
            }

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        private List<IEventHandlerFactory> GetOrCreateHandlerFactories(Type eventType)
        {
            return HandlerFactories.GetOrAdd(
                eventType,
                type =>
                {
                    var eventName = EventNameAttribute.GetNameOrDefault(type);
                    EventTypes[eventName] = type;
                    return new List<IEventHandlerFactory>();
                }
            );
        }

        /// <summary>
        /// 如何RabbitMQ上处理取消订阅以解除绑定、暂时不会写
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="factory"></param>
        public void Unsubscribe(Type eventType, IEventHandlerFactory factory)
        {
            throw new NotImplementedException();
        }
    }
}
