using Easy.Core.Flow.UnitOfWork.Uow.Providers;
using Easy.Core.UnitOfWork.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers
{
    public class UnitOfWorkDbContextProvider : IUnitOfWorkDbContextProvider
    {
        protected readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public UnitOfWorkDbContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public virtual DbContext GetDbContext()
        {
            return _currentUnitOfWorkProvider.Current.GetDbContext();
        }
    }
}
