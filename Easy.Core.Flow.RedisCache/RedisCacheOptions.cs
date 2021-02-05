using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Easy.Core.Flow.RedisCache
{
    public class RedisCacheOptions
    {
        private const string ConnectionStringKey = "Redis.Cache";

        private const string DatabaseIdSettingKey = "Redis.Cache.DatabaseId";
        
        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; }

        public RedisCacheOptions()
        {
            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        private static int GetDefaultDatabaseId()
        {
            var appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            if (appSetting == null || appSetting.Trim() == "")
            {
                return -1;
            }

            int databaseId;
            if (!int.TryParse(appSetting, out databaseId))
            {
                return -1;
            }

            return databaseId;
        }

        private static string GetDefaultConnectionString()
        {
            var connStr = ConfigurationManager.ConnectionStrings[ConnectionStringKey];
            if (connStr == null)
            {
                return "localhost";
            }

            return connStr.ConnectionString;
        }

    }
}
