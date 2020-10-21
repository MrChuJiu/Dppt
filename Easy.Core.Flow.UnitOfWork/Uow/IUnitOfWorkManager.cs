using Easy.Core.Flow.UnitOfWork.Uow.Handles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 工作单元管理器
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// 获取当前激活的工作单元,如果不存在则返回空
        /// </summary>
        // IActiveUnitOfWork Current { get; }

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <returns>工作单元处理器</returns>
        IUnitOfWorkCompleteHandle Begin();

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <param name="scope">事务配置</param>
        /// <returns>工作单元处理器</returns>
        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);

        /// <summary>
        /// 启动一个新的工作单元
        /// </summary>
        /// <param name="options">工作单元配置</param>
        /// <returns>工作单元处理器</returns>
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}
