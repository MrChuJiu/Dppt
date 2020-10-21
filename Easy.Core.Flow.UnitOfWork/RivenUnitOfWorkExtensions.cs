using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;
using JetBrains.Annotations;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.Flow.AspectCore;

namespace Easy.Core.Flow.UnitOfWork
{
    public static class RivenUnitOfWorkExtensions
    {
        /// <summary>
        /// 添加Riven UnitOfWork 服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRivenUnitOfWork([NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.TryAddTransient<IUnitOfWorkManager, DefaultUnitOfWorkManager>();
            services.TryAddTransient<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>();
            // services.TryAddTransient<IActiveTransactionProvider, NullActiveTransactionProvider>();

            services.TryAddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            services.TryAddSingleton<IConnectionStringStore, DefaultConnectionStringStore>();

            return services;
        }

        /// <summary>
        /// 添加当前连接字符串名称提供者
        /// </summary>
        /// <typeparam name="TProvider">提供者实现</typeparam>
        /// <param name="services">服务类实例</param>
        /// <returns></returns>
        public static IServiceCollection AddRivenCurrentConnectionStringNameProvider<TProvider>([NotNull] this IServiceCollection services)
            where TProvider : class, ICurrentConnectionStringNameProvider
        {
            Check.NotNull(services, nameof(services));

            services.TryAddScoped<ICurrentConnectionStringNameProvider, TProvider>();

            return services;
        }
        /// <summary>
        /// 添加默认连接字符
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static IServiceCollection AddDefaultConnectionString([NotNull] this IServiceCollection services, [NotNull] string connectionString)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));
            return services.AddConnectionString(RivenUnitOfWorkConsts.DefaultConnectionStringName, connectionString);
        }

        /// <summary>
        /// 添加数据库连接字符串
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">连接字符串名称</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static IServiceCollection AddConnectionString([NotNull] this IServiceCollection services, [NotNull] string name, [NotNull] string connectionString)
        {
            Check.NotNull(services, nameof(services));
            Check.NotNullOrEmpty(name, nameof(name));
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));

            var count = services.Where(o => o.ImplementationInstance is IConnectionStringProvider)
                                .Select(o => (o.ImplementationInstance as IConnectionStringProvider))
                                .Count(o => o.Name == name);
            if (count > 0)
            {
                throw new ArgumentException($"A connection string with the name {name} already exists");
            }

            var connectionStringProvider = new ConnectionStringProvider(name, connectionString);
            services.AddSingleton<IConnectionStringProvider>(connectionStringProvider);

            return services;
        }
        /// <summary>
        /// 添加数据库连接字符串
        /// </summary>
        /// <param name="services"></param>
        /// <param name="name">连接字符串名称</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <returns></returns>
        public static IServiceProvider AddConnectionString([NotNull] this IServiceProvider serviceProvider, [NotNull] string name, [NotNull] string connectionString)
        {
            Check.NotNull(serviceProvider, nameof(serviceProvider));
            Check.NotNullOrEmpty(name, nameof(name));
            Check.NotNullOrEmpty(connectionString, nameof(connectionString));

            var connectionStringStore = serviceProvider.GetService<IConnectionStringStore>();

            var connectionStringProvider = new ConnectionStringProvider(name, connectionString);
            connectionStringStore.CreateOrUpdate(connectionStringProvider);


            return serviceProvider;
        }

    }
}
