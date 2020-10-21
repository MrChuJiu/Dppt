using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork.Uow
{
    public interface IConnectionStringResolver
    {
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name">连接字符串标识名称</param>
        /// <returns>连接字符串</returns>
        string Resolve(string name);
    }
}
