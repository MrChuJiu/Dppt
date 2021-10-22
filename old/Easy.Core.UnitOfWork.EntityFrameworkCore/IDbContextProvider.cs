using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.UnitOfWork.EntityFrameworkCore
{
    /// <summary>
    /// DbContext Provider
    /// </summary>
    public interface IDbContextProvider
    {
        /// <summary>
        /// DbContext 标识名称,默认为空字符串
        /// </summary>
        string Name { get; }

        /// <summary>
        /// DbContext 类型
        /// </summary>
        Type DbContextType { get; }


        /// <summary>
        /// 配置函数
        /// </summary>
        Action<DbContextConfiguration> Configuration { get; }
    }

    public class DbContextProvider : IDbContextProvider
    {
        public string Name { get; private set; }

        public Type DbContextType { get; private set; }

        public Action<DbContextConfiguration> Configuration { get; private set; }



        public DbContextProvider(string name, Type dbContextType, Action<DbContextConfiguration> configuration)
        {
            Name = name;
            DbContextType = dbContextType;
            Configuration = configuration;
        }

    }
}
