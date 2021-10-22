using Easy.Core.Flow.UnitOfWork.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore
{
    /// <summary>
    /// 数据库上下文配置信息
    /// </summary>
    public class DbContextConfiguration
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public virtual string ConnectionString { get; protected set; }

        /// <summary>
        /// 已存在的连接
        /// </summary>
        public virtual DbConnection ExistingConnection { get; protected set; }

        /// <summary>
        /// 数据库选项配置器
        /// </summary>
        public virtual DbContextOptionsBuilder DbContextOptions { get; }

        /// <summary>
        /// 工作单元选项
        /// </summary>
        public virtual UnitOfWorkOptions UnitOfWorkOptions { get; protected set; }

        public DbContextConfiguration(
            string connectionString,
            DbConnection existingConnection,
            UnitOfWorkOptions unitOfWorkOptions)
        {
            ConnectionString = connectionString;
            ExistingConnection = existingConnection;
            UnitOfWorkOptions = unitOfWorkOptions;

            DbContextOptions = new DbContextOptionsBuilder();
        }
    }
}
