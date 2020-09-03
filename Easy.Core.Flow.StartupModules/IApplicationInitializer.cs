using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Easy.Core.Flow.StartupModules
{
    /// <summary>
    /// 表示在启动期间初始化应用程序服务的类
    /// </summary>
    public interface IApplicationInitializer
    {
        /// <summary>
        /// Invokes the <see cref="IApplicationInitializer"/> instance.
        /// </summary>
        Task Invoke();
    }
}
