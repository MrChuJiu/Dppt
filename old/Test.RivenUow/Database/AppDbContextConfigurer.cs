using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Test.RivenUow.Database
{
    public static class AppDbContextConfigurer
    {
        /// <summary>
        /// 配置DbContext
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="connection">连接字符串</param>
        public static void Configure(
            this DbContextOptionsBuilder builder,
            string connectionString)
        {
            builder.UseSqlServer(connectionString, (options) =>
            {
                options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
            });
        }

        /// <summary>
        /// 配置DbContext
        /// </summary>
        /// <param name="builder">配置器</param>
        /// <param name="connection">现有连接</param>
        public static void Configure(
            this DbContextOptionsBuilder builder,
            DbConnection connection)
        {
            builder.UseSqlServer(connection, (options) =>
            {
                options.MigrationsHistoryTable(AppConsts.Database.MigrationsHistoryTableName);
            });
        }
    }
}
