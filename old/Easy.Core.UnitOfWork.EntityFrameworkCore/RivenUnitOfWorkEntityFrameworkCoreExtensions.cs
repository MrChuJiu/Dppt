using Easy.Core.Flow.AspectCore;
using Easy.Core.Flow.Domain.Repositories;
using Easy.Core.Flow.UnitOfWork;
using Easy.Core.Flow.UnitOfWork.EntityFrameworkCore.Repositories;
using Easy.Core.Flow.UnitOfWork.EntityFrameworkCore.Uow.Providers;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
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

            services.TryAddTransient<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();

            services.TryAddTransient<IUnitOfWork, EfCoreUnitOfWork>();

            services.TryAddTransient<IActiveTransactionProvider, EfCoreActiveTransactionProvider>();

            services.AddRivenUnitOfWork();

            return services;
        }
        /// <summary>
        /// 添加EFCore实现的UnitOfWork 支持的仓储
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWorkWithEntityFrameworkCoreRepository(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(IRepository<>), typeof(EfCoreRepositoryBase<>));
            services.TryAddTransient(typeof(IRepository<,>), typeof(EfCoreRepositoryBase<,>));


            return services;
        }

        /// <summary>
        ///  添加EFCore实现的UnitOfWork 默认支持的DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="configurationAction">配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWorkWithEntityFrameworkCoreDefaultDbContext<TDbContext>(this IServiceCollection services, Action<DbContextConfiguration> configurationAction)
            where TDbContext : DbContext
        {
            return services.AddUnitOfWorkWithEntityFrameworkCoreDbContext<TDbContext>(RivenUnitOfWorkEntityFrameworkCoreConsts.DefaultDbContextProviderName, configurationAction);
        }

        /// <summary>
        ///  添加EFCore实现的UnitOfWork 支持的DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="name">DbContext标识的名称</param>
        /// <param name="configurationAction">配置函数</param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWorkWithEntityFrameworkCoreDbContext<TDbContext>(this IServiceCollection services, string name, Action<DbContextConfiguration> configurationAction)
           where TDbContext : DbContext
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configurationAction, nameof(configurationAction));

            var count = services.Where(o => o.ImplementationInstance is DbContextProvider)
                .Select(o => (o.ImplementationInstance as DbContextProvider))
                .Count(o => o.Name == name);

            if (count > 0)
            {
                throw new ArgumentException($"A DbContext with the name {name} already exists");
            }

            var dbContextType = typeof(TDbContext);

            var unitOfWorkDbContextProvider = new DbContextProvider(name, dbContextType, configurationAction);
            services.AddSingleton<IDbContextProvider>(unitOfWorkDbContextProvider);

            return services;
        }


    }
}
