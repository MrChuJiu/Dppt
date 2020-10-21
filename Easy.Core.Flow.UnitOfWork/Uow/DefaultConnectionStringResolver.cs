
using Easy.Core.Flow.AspectCore;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Linq;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        protected readonly Dictionary<string, IConnectionStringProvider> _connectionStringProviderDict;

        protected readonly IConnectionStringStore _connectionStringStore;

        public DefaultConnectionStringResolver(IServiceProvider service, IConnectionStringStore connectionStringStore)
        {
            _connectionStringProviderDict = service.GetServices<IConnectionStringProvider>()
                .ToDictionary(o => o.Name);
            _connectionStringStore = connectionStringStore;
        }



        public string Resolve([NotNull] string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            var connectionStringProvider = _connectionStringStore.Get(name);
            if (connectionStringProvider != null)
            {
                return connectionStringProvider.ConnectionString;
            }

            if (this._connectionStringProviderDict.TryGetValue(name, out connectionStringProvider))
            {
                return connectionStringProvider.ConnectionString;
            }

            name = RivenUnitOfWorkConsts.DefaultConnectionStringName;

            connectionStringProvider = _connectionStringStore.Get(name);
            if (connectionStringProvider != null)
            {
                return connectionStringProvider.ConnectionString;
            }

            if (this._connectionStringProviderDict.TryGetValue(name, out connectionStringProvider))
            {
                return connectionStringProvider.ConnectionString;
            }


            throw new ArgumentException($"具有默认名称的连接字符串不存在");
        }
    }
}
