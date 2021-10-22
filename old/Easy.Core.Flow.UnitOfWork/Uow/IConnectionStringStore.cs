using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using Easy.Core.Flow.AspectCore;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 连接字符串 Store
    /// </summary>
    public interface IConnectionStringStore
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        IQueryable<IConnectionStringProvider> GetAll();

        /// <summary>
        /// 根据名称获取
        /// </summary>
        /// <param name="connectionStringProviderName"></param>
        /// <returns></returns>
        IConnectionStringProvider Get(string connectionStringProviderName);

        /// <summary>
        /// 创建或更新
        /// </summary>
        /// <param name="connectionStringProvider"></param>
        void CreateOrUpdate(IConnectionStringProvider connectionStringProvider);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="connectionStringProviderName"></param>
        IConnectionStringProvider Remove(string connectionStringProviderName);
    }

    public class DefaultConnectionStringStore : IConnectionStringStore
    {
        protected static ConcurrentDictionary<string, IConnectionStringProvider> _data = new ConcurrentDictionary<string, IConnectionStringProvider>();


        public void CreateOrUpdate(IConnectionStringProvider connectionStringProvider)
        {
            Check.NotNull(connectionStringProvider, nameof(connectionStringProvider));
            Check.NotNullOrWhiteSpace(connectionStringProvider.Name, nameof(connectionStringProvider.Name));
            Check.NotNullOrWhiteSpace(connectionStringProvider.ConnectionString, nameof(connectionStringProvider.ConnectionString));

            _data[connectionStringProvider.Name] = connectionStringProvider;
        }

        public IConnectionStringProvider Get(string connectionStringProviderName)
        {
            Check.NotNullOrWhiteSpace(connectionStringProviderName, nameof(connectionStringProviderName));

            if (_data.TryGetValue(connectionStringProviderName, out IConnectionStringProvider connectionStringProvider))
            {
                return connectionStringProvider;
            }

            return null;
        }

        public IQueryable<IConnectionStringProvider> GetAll()
        {
            return _data.Select(o => o.Value).AsQueryable();
        }

        public IConnectionStringProvider Remove(string connectionStringProviderName)
        {
            Check.NotNullOrWhiteSpace(connectionStringProviderName, nameof(connectionStringProviderName));

            if (_data.TryRemove(connectionStringProviderName, out IConnectionStringProvider connectionStringProvider))
            {
                return connectionStringProvider;
            }

            return null;
        }
    }
}
