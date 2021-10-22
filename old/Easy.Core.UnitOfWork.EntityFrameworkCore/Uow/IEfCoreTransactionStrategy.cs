using Easy.Core.Flow.UnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Uow
{
    /// <summary>
    /// EF Core 事务策略
    /// </summary>
    public interface IEfCoreTransactionStrategy : IDisposable
    {
        /// <summary>
        /// 初始化工作单元选项
        /// </summary>
        /// <param name="options"></param>
        void InitOptions(UnitOfWorkOptions options);

        /// <summary>
        /// 创建数据库上下文对象
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbContextResolver">数据库上下文实例化器</param>
        /// <returns></returns>
        TDbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver, string dbContextProviderName)
            where TDbContext : DbContext;

        /// <summary>
        /// 创建数据库上下文对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dbContextResolver">数据库上下文实例化器</param>
        /// <returns></returns>
        DbContext CreateDbContext(string connectionString, IDbContextResolver dbContextResolver, string dbContextProviderName);

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();
    }
}
