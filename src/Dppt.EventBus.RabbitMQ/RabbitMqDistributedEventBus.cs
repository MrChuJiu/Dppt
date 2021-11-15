using Dppt.Core.Dppt;
using Dppt.Core.Dppt.Collections;
using Dppt.Core.Dppt.Threading;
using Dppt.EventBus.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
        /// <summary>
        /// rabbmitmq 连接服务
        /// </summary>
        public readonly IRabbitMqConnections _rabbitMqConnections;

        /// <summary>
        /// 用于配置服务的交换名称
        /// </summary>
        protected DpptRabbitMqEventBusOptions RabbitMqEventBusOptions { get; }

        /// <summary>
        /// 交换机相关参数会在Initialize被初始化用于构造消费者
        /// </summary>
        protected ExchangeDeclareConfiguration Exchange { get; private set; }
        /// <summary>
        /// 队列相关参数Initialize被初始化用于构造消费者
        /// </summary>
        protected QueueDeclareConfiguration Queue { get; private set; }
        /// <summary>
        /// 构建给消费者使用
        /// </summary>
        protected IModel Channel { get; private set; }
        /// <summary>
        /// 该队列用于bind队列
        /// </summary>
        protected ConcurrentQueue<QueueBindCommand> QueueBindCommands { get; }
        /// <summary>
        /// 服务工厂
        /// </summary>
        protected IServiceScopeFactory ServiceScopeFactory { get; }
        /// <summary>
        /// 根据实体类型存放Handler服务工厂
        /// </summary>
        protected ConcurrentDictionary<Type, List<IEventHandlerFactory>> HandlerFactories { get; }
        /// <summary>
        /// key = eventName，value = handler
        /// </summary>
        protected ConcurrentDictionary<string, Type> EventTypes { get; }
        /// <summary>
        /// Hanler注册配置
        /// </summary>
        protected DistributedEventBusOptions DistributedEventBusOptions { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected object ChannelSendSyncLock { get; } = new object();



        public RabbitMqDistributedEventBus(IRabbitMqConnections rabbitMqConnections, IOptions<DpptRabbitMqEventBusOptions> options, IOptions<DistributedEventBusOptions> distributedEventBusOptions, IServiceScopeFactory serviceScopeFactory, IServiceProvider serviceProvider)
        {
            _rabbitMqConnections = rabbitMqConnections;
            RabbitMqEventBusOptions = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
            DistributedEventBusOptions = distributedEventBusOptions.Value;
            HandlerFactories = new ConcurrentDictionary<Type, List<IEventHandlerFactory>>();
            EventTypes = new ConcurrentDictionary<string, Type>();
            QueueBindCommands = new ConcurrentQueue<QueueBindCommand>();
            ServiceProvider = serviceProvider;
        }



        public Task PublishAsync<TEvent>(TEvent eventData)
        {
            var eventName = EventNameAttribute.GetNameOrDefault(typeof(TEvent));
            var body = JsonSerializer.Serialize(eventData);
            return PublishAsync(eventName, body, null, null);
        }

        public Task PublishAsync(Type eventType, object eventData, IBasicProperties properties, Dictionary<string, object> headersArguments = null)
        {
            var eventName = EventNameAttribute.GetNameOrDefault(eventType);
            var body = JsonSerializer.Serialize(eventData);

            return PublishAsync(eventName, body, properties, headersArguments);
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





        public void Initialize()
        {

            Exchange = new ExchangeDeclareConfiguration(RabbitMqEventBusOptions.ExchangeName,"direct",true);
            Queue = new QueueDeclareConfiguration(RabbitMqEventBusOptions.ClientName, true, false, false);

            // 启动一个消费者
            if (!_rabbitMqConnections.IsConnected)
            {
                _rabbitMqConnections.TryConnect();
            }

            try
            {

                Channel = _rabbitMqConnections.CreateModel();



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

                SubscribeHandlers(DistributedEventBusOptions.Handlers);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
            }
        }

        /// <summary>
        /// 收到消息后 通过反射查找相应的handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="basicDeliverEventArgs"></param>
        /// <returns></returns>
        protected virtual async Task HandleIncomingMessageAsync(object sender, BasicDeliverEventArgs basicDeliverEventArgs)
        {
            try
            {
                var eventName = basicDeliverEventArgs.RoutingKey;
                var eventType = EventTypes.GetOrDefault(eventName);
                if (eventType == null)
                {
                    return;
                }
                var eventBytes = basicDeliverEventArgs.Body.ToArray();
                // 调用实现方式
                var eventData = JsonSerializer.Deserialize(eventBytes, eventType);
                await TriggerHandlersAsync(eventType, eventData);

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
                        SubscribeAsync(genericArgs[0], new IocEventHandlerFactory(ServiceScopeFactory, handler));
                    }
                }
            }
        }

        public  async Task<IDisposable> SubscribeAsync(Type eventType, IEventHandlerFactory factory)
        {
            var handlerFactories = GetOrCreateHandlerFactories(eventType);

            if (factory.IsInFactories(handlerFactories))
            {
                return NullDisposable.Instance;
            }

            handlerFactories.Add(factory);

            if (handlerFactories.Count == 1) //TODO: Multi-threading!
            {
                QueueBindCommands.Enqueue(new QueueBindCommand(QueueBindType.Bind, EventNameAttribute.GetNameOrDefault(eventType)));
                await TrySendQueueBindCommandsAsync();
            }

            return new EventHandlerFactoryUnregistrar(this, eventType, factory);
        }

        private async Task TrySendQueueBindCommandsAsync()
        {
            try
            {
                while (!QueueBindCommands.IsEmpty)
                {
                    if (Channel == null || Channel.IsClosed)
                    {
                        return;
                    }

                    lock (ChannelSendSyncLock)
                    {
                        if (QueueBindCommands.TryPeek(out var command))
                        {
                            switch (command.Type)
                            {
                                case QueueBindType.Bind:
                                    Channel.QueueBind(
                                        queue: Queue.QueueName,
                                        exchange: Exchange.ExchangeName,
                                        routingKey: command.RoutingKey
                                    );
                                    break;
                                case QueueBindType.Unbind:
                                    Channel.QueueUnbind(
                                        queue: Queue.QueueName,
                                        exchange: Exchange.ExchangeName,
                                        routingKey: command.RoutingKey
                                    );
                                    break;
                                default:
                                    throw new AbpException($"Unknown {nameof(QueueBindType)}: {command.Type}");
                            }

                            QueueBindCommands.TryDequeue(out command);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
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
            GetOrCreateHandlerFactories(eventType).Locking(factories => factories.Remove(factory));
        }

        public async Task PublishAsync(Type eventType, object eventData, bool onUnitOfWorkComplete = true, bool useOutbox = true)
        {
            if (useOutbox)
            {
                if (await AddToOutboxAsync(eventType, eventData))
                {
                    return;
                }
            }

            await PublishToEventBusAsync(eventType, eventData);
        }

        protected  async Task PublishToEventBusAsync(Type eventType, object eventData)
        {
            await PublishAsync(eventType, eventData, null);
        }


        private async Task<bool> AddToOutboxAsync(Type eventType, object eventData)
        {

            foreach (var outboxConfig in DistributedEventBusOptions.Outboxes.Values)
            {
                if (outboxConfig.Selector == null || outboxConfig.Selector(eventType))
                {
                    var eventOutbox = (IEventOutbox)ServiceProvider.GetRequiredService(outboxConfig.ImplementationType);
                    var eventName = EventNameAttribute.GetNameOrDefault(eventType);
                    await eventOutbox.EnqueueAsync(
                        new OutgoingEventInfo(
                            Guid.NewGuid(),
                            eventName,
                            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(eventData)),
                            DateTime.Now
                        )
                    );
                    return true;
                }
            }

            return false;
        }


    }



    public class QueueBindCommand
    {
        public QueueBindType Type { get; }

        public string RoutingKey { get; }

        public QueueBindCommand(QueueBindType type, string routingKey)
        {
            Type = type;
            RoutingKey = routingKey;
        }
    }
    public enum QueueBindType
    {
        Bind,
        Unbind
    }
}
