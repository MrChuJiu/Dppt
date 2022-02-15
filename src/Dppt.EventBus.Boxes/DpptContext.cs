using Dppt.EventBus.Boxes.Entities;
using Dppt.EventBus.Distributed;
using Dppt.EventBus.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dppt.EventBus.Boxes
{
    public abstract class DpptContext<TDbContext> : DbContext where TDbContext : DbContext
    {
        protected IServiceProvider ServiceProvider { get; }

        private IDistributedEventBus DistributedEventBus => ServiceProvider.GetRequiredService<IDistributedEventBus>();

        private ILocalEventBus LocalEventsBus => ServiceProvider.GetRequiredService<ILocalEventBus>();

        protected DpptContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var eventReport = CreateEventReport();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            await PublishEntityEventsAsync(eventReport);

            return result;
        }

        protected virtual EntityEventReport CreateEventReport()
        {
            var eventReport = new EntityEventReport();


            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                var generatesDomainEventsEntity = entry.Entity as IGeneratesDomainEvents;
                if (generatesDomainEventsEntity == null)
                {
                    continue;
                }

                var localEvents = generatesDomainEventsEntity.GetLocalEvents()?.ToArray();
                if (localEvents != null && localEvents.Any())
                {
                    eventReport.DomainEvents.AddRange(
                        localEvents.Select(
                            eventRecord => new DomainEventEntry(
                                entry.Entity,
                                eventRecord.EventData,
                                eventRecord.EventOrder
                            )
                        )
                    );
                    generatesDomainEventsEntity.ClearLocalEvents();
                }


                var distributedEvents = generatesDomainEventsEntity.GetDistributedEvents()?.ToArray();
                if (distributedEvents != null && distributedEvents.Any())
                {
                    eventReport.DistributedEvents.AddRange(
                        distributedEvents.Select(
                            eventRecord => new DomainEventEntry(
                                entry.Entity,
                                eventRecord.EventData,
                                eventRecord.EventOrder)
                        )
                    );
                    generatesDomainEventsEntity.ClearDistributedEvents();
                }

            }

            return eventReport;

        }

        private async Task PublishEntityEventsAsync(EntityEventReport changeReport)
        {
            foreach (var localEvent in changeReport.DomainEvents)
            {
                await LocalEventsBus.PublishAsync(localEvent.EventData.GetType(), localEvent.EventData);
            }

            foreach (var distributedEvent in changeReport.DistributedEvents)
            {

                await DistributedEventBus.PublishAsync(
                  distributedEvent.EventData.GetType(),
                  distributedEvent.EventData,
                  onUnitOfWorkComplete: false,
                  useOutbox: true
              );
            }
        }
    }
}
