using Easy.Core.Domain.Entities;
using Easy.Core.Flow.Domain.Repositories;
using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TEntity> : EfCoreRepositoryBase<TEntity, long>, IRepository<TEntity>
           where TEntity : class, IEntity<long>
    {
        public EfCoreRepositoryBase(IActiveTransactionProvider transactionProvider, IUnitOfWorkDbContextProvider unitOfWorkDbContextProvider)
            : base(transactionProvider, unitOfWorkDbContextProvider)
        {
        }
    }
}
