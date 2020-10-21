using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore.Uow
{
    /// <summary>
    /// 激活的事务信息
    /// </summary>
    public class ActiveTransactionInfo
    {
        /// <summary>
        /// 数据库上下文事务
        /// </summary>
        public IDbContextTransaction DbContextTransaction { get; }

        /// <summary>
        /// 起始的数据库上下文
        /// </summary>
        public DbContext StarterDbContext { get; }

        /// <summary>
        /// 附加的数据库上下文集合
        /// </summary>
        public List<DbContext> AttendedDbContexts { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextTransaction">数据库上下文的事务</param>
        /// <param name="starterDbContext">起始的数据库上下文</param>
        public ActiveTransactionInfo(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<DbContext>();
        }
    }
}
