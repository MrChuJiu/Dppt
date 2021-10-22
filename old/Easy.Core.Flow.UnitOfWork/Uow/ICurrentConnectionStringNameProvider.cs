using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    /// <summary>
    /// 当前连接字符串名称提供者
    /// </summary>
    public interface ICurrentConnectionStringNameProvider
    {
        /// <summary>
        /// 当前
        /// </summary>
        string Current { get; }
    }
}
