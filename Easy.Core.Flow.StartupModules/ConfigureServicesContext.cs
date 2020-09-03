using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    /// <summary>
    /// 配置服务上下文
    /// </summary>
    public class ConfigureServicesContext
    {
        public ConfigureServicesContext(IConfiguration configuration, IWebHostEnvironment hostingEnvironment, StartupModulesOptions options)
        {
            Configuration = configuration;
            HostingEnvironment = hostingEnvironment;
            Options = options;
        }
        /// <summary>
        /// 获取Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 获取 IWebHostEnvironment
        /// </summary>
        public IWebHostEnvironment HostingEnvironment { get; }

        /// <summary>
        /// 获取启动模块
        /// </summary>
        public StartupModulesOptions Options { get; }
    }
}
