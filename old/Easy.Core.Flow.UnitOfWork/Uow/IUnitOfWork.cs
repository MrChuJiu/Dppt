using Easy.Core.Flow.UnitOfWork.Uow.Handles;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// 工作单元id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 外部UOW的引用.
        /// </summary>
        IUnitOfWork Outer { get; set; }

        /// <summary>
        /// 根据工作单元选项创建
        /// </summary>
        /// <param name="options">Unit of work options</param>
        void Begin(UnitOfWorkOptions options);
    }
}
