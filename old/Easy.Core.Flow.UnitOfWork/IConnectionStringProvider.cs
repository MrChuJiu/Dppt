using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.UnitOfWork
{
    /// <summary>
    /// 数据
    /// </summary>
    public interface IConnectionStringProvider
    {
        string Name { get; }

        string ConnectionString { get; }
    }

    public class ConnectionStringProvider : IConnectionStringProvider
    {
        public ConnectionStringProvider(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public string Name { get; protected set; }

        public string ConnectionString { get; protected set; }


    }
}
