using Easy.Core.Flow.UnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore
{
    public interface IDbContextResolver
    {
        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <typeparam name="TDbContext">数据库上下文类型</typeparam>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="existingConnection">已存在的数据库连接</param>
        /// <param name="unitOfWorkOptions">当前工作单元选项</param>
        /// <param name="dbContextProviderName">DbContext Provider Name</param>
        /// <returns>数据库上下文</returns>
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection, UnitOfWorkOptions unitOfWorkOptions, string dbContextProviderName)
          where TDbContext : DbContext;

        /// <summary>
        /// 创建数据库上下文
        /// </summary>
        /// <param name="dbContextType">数据库上下文类型</param>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="existingConnection">已存在的数据库连接</param>
        /// <param name="unitOfWorkOptions">当前工作单元选项</param>
        /// <param name="dbContextProviderName">DbContext Provider Name</param>
        /// <returns>数据库上下文</returns>
        DbContext Resolve(string connectionString, DbConnection existingConnection, UnitOfWorkOptions unitOfWorkOptions, string dbContextProviderName);
    }
}
