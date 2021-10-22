using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.Uow.Providers
{
    public interface IActiveTransactionProvider
    {
        /// <summary>
        /// 获取当前的事务
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbTransaction GetActiveTransaction();

        /// <summary>
        /// 获取当前的数据库连接
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        IDbConnection GetActiveConnection();
    }
}
