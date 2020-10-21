using Easy.Core.Flow.UnitOfWork.Uow.Handles;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

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
            throw new NotImplementedException();
        }
    }
}
