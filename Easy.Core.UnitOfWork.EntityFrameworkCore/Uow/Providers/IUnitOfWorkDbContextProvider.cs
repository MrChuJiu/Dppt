using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Uow.Providers
{
    /// <summary>
    /// 当前工作单元下的 DbContext
    /// </summary>
    public interface IUnitOfWorkDbContextProvider
    {
        /// <summary>
        /// 获取当前工作单元的 DbContext
        /// </summary>
        /// <returns></returns>
        DbContext GetDbContext();
    }
}
