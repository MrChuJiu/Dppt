using Dppt.EventBus.Boxes.Entities;
using Microsoft.EntityFrameworkCore;
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
        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {

            var eventReport = CreateEventReport();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            PublishEntityEvents(eventReport);

            return result;
        }



        protected virtual EntityEventReport CreateEventReport() {
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

        private void PublishEntityEvents(EntityEventReport changeReport)
        {
            foreach (var localEvent in changeReport.DomainEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceLocalEvent(
                    new UnitOfWorkEventRecord(localEvent.EventData.GetType(), localEvent.EventData, localEvent.EventOrder)
                );
            }

            foreach (var distributedEvent in changeReport.DistributedEvents)
            {
                UnitOfWorkManager.Current?.AddOrReplaceDistributedEvent(
                    new UnitOfWorkEventRecord(distributedEvent.EventData.GetType(), distributedEvent.EventData, distributedEvent.EventOrder)
                );
            }
        }
    }
}
