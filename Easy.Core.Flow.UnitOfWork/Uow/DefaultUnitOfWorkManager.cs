using Easy.Core.Flow.UnitOfWork.Uow.Handles;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    public class DefaultUnitOfWorkManager : IUnitOfWorkManager
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public IActiveUnitOfWork Current => _currentUnitOfWorkProvider.Current;

        public DefaultUnitOfWorkManager(
            IServiceProvider serviceProvider,
             ICurrentUnitOfWorkProvider currentUnitOfWorkProvider
            )
        {
            _serviceProvider = serviceProvider;
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }
        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
        {
            return Begin(new UnitOfWorkOptions { Scope = scope });
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            var outerUow = _currentUnitOfWorkProvider.Current;

            if (options.Scope == TransactionScopeOption.Required && outerUow != null)
            {
                return new InnerUnitOfWorkCompleteHandle();
            }
            var uow = _serviceProvider.GetService<IUnitOfWork>();
            uow.Completed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Failed += (sender, args) =>
            {
                _currentUnitOfWorkProvider.Current = null;
            };

            uow.Disposed += (sender, args) =>
            {
                uow.Dispose();
            };

            uow.Begin(options);
            // 从外部UOW继承connectionStringName todo 理解为嵌套工作单元
            if (outerUow != null)
            {
                uow.SetConnectionStringName(outerUow.GetConnectionStringName());
            }

            _currentUnitOfWorkProvider.Current = uow;


            return uow;
        }
    }
}
