using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Easy.Core.Flow.UnitOfWork.Uow.Handles
{
    /// <summary>
    /// 工作单元事件处理器
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// 提交
        /// </summary>
        void Complete();

        /// <summary>
        /// 提交 - 异步
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CompleteAsync(CancellationToken cancellationToken = default);
    }
}
