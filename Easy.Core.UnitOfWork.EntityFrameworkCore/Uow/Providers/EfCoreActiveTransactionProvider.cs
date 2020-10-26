using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.EntityFrameworkCore.Uow.Providers
{
    public class EfCoreActiveTransactionProvider : IActiveTransactionProvider
    {
        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public EfCoreActiveTransactionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public IDbConnection GetActiveConnection()
        {
            return GetDbContext().Database.GetDbConnection();
        }

        public IDbTransaction GetActiveTransaction()
        {
            return GetDbContext().Database.CurrentTransaction?.GetDbTransaction();
        }

        protected virtual DbContext GetDbContext()
        {
            return _currentUnitOfWorkProvider.Current.GetDbContext();
        }
    }
}
