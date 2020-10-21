using Easy.Core.Flow.UnitOfWork;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore
{
    public static class RivenUnitOfWorkEntityFrameworkCoreExtensions
    {
        /// <summary>
        /// 添加EFCore实现的UnitOfWork
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWorkWithEntityFrameworkCore(this IServiceCollection services)
        {
            services.TryAddTransient<IDbContextResolver, DefaultDbContextResolver>();

            services.TryAddTransient<IUnitOfWorkDbContextProvider, UnitOfWorkDbContextProvider>();

            //services.TryAddTransient<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();

            //services.TryAddTransient<IUnitOfWork, EfCoreUnitOfWork>();

            //services.TryAddTransient<IActiveTransactionProvider, EfCoreActiveTransactionProvider>();

            services.AddRivenUnitOfWork();

            return services;
        }

    }
}
