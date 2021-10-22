using Easy.Core.Flow.AspectCore;
using Easy.Core.Flow.UnitOfWork.Uow;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Uow
{
    public class EfCoreUnitOfWork : UnitOfWorkBase
    {
        protected string _dbContextProviderName;

        protected readonly IDictionary<string, DbContext> _activeDbContexts;

        protected readonly IServiceProvider _serviceProvider;

        protected readonly IEfCoreTransactionStrategy _transactionStrategy;

        protected readonly IDbContextResolver _dbContextResolver;
        protected readonly IConnectionStringResolver _connectionStringResolver;

        IServiceProvider ServiceProvider => this._serviceProvider;
        IDictionary<string, DbContext> ActiveDbContexts => this._activeDbContexts;




        public EfCoreUnitOfWork(
            IServiceProvider serviceProvider,
            IEfCoreTransactionStrategy transactionStrategy,
            IDbContextResolver dbContextResolver,
            IConnectionStringResolver connectionStringResolver)
        {
            _serviceProvider = serviceProvider;
            _transactionStrategy = transactionStrategy;
            _dbContextResolver = dbContextResolver;
            _connectionStringResolver = connectionStringResolver;

            _activeDbContexts = new Dictionary<string, DbContext>();
        }

        /// <summary>
        /// 获取或者创建数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <returns>数据库上下文对象</returns>
        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
           where TDbContext : DbContext
        {
            var dbContext = this.GetOrCreateDbContext();

            return (TDbContext)dbContext;
        }

        /// <summary>
        /// 获取或创建数据库上下文
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="name">名称</param>
        /// <returns>数据库上下文对象</returns>
        public virtual DbContext GetOrCreateDbContext()
        {
            // 获取连接字符串
            var nameOrConnectionString = _connectionStringResolver.Resolve(this.GetConnectionStringName());

            // 缓存键值
            var dbContextKey = this.GetDbContextKey(nameOrConnectionString);

            DbContext dbContext;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContext))
            {
                if (Options.IsTransactional == true)
                {
                    dbContext = _transactionStrategy.CreateDbContext(nameOrConnectionString, _dbContextResolver, this._dbContextProviderName);
                }
                else
                {
                    dbContext = _dbContextResolver.Resolve(nameOrConnectionString, null, this.Options, this._dbContextProviderName);
                }

                if (Options.Timeout.HasValue &&
                    dbContext.Database.IsRelational() &&
                    !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    var commandTimeout = Convert.ToInt32(Options.Timeout.Value.TotalSeconds);

                    //dbContext.Database.SetCommandTimeout(Options.Timeout.Value.TotalSeconds.To<int>());
                    dbContext.Database.SetCommandTimeout(commandTimeout);
                }

                //TODO: Object materialize event
                //TODO: Apply current filters to this dbcontext

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return dbContext;
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.InitOptions(Options);
            }

            this._dbContextProviderName = this.Options.GetDbContextProviderName();
        }

        public override void SaveChanges()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                SaveChangesInDbContext(dbContext);
            }
        }

        public override async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                await SaveChangesInDbContextAsync(dbContext, cancellationToken);
            }
        }

        /// <summary>
        /// 设置 DbContext Provider Name
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public virtual IDisposable SetDbContextProvider(string providerName)
        {
            var oldDbContextProviderName = this._dbContextProviderName;
            this._dbContextProviderName = providerName;

            return new DisposeAction(() =>
            {
                this._dbContextProviderName = oldDbContextProviderName;
            });
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CompleteUowAsync(CancellationToken cancellationToken = default)
        {
            await SaveChangesAsync(cancellationToken);
            CommitTransaction();
        }

        private void CommitTransaction()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }
        }



        protected override void DisposeUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Dispose();
            }
            else
            {
                foreach (var context in GetAllActiveDbContexts())
                {
                    Release(context);
                }
            }

            ActiveDbContexts.Clear();
        }


        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext, CancellationToken cancellationToken = default)
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
        }

        protected virtual string GetDbContextKey(string nameOrConnectionString)
        {
            var dbContextKey = $"{this._dbContextProviderName}#{nameOrConnectionString}";

            return dbContextKey;

        }

    }
}
