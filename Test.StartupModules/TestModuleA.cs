using Easy.Core.Flow.RivenModular;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.StartupModules
{
    [DependsOn(
  typeof(TestModuleB)
    )]
    public class TestModuleA : AppModule
    {
        public override void OnPreConfigureServices(ServiceConfigurationContext context)
        {
            // 注册服务之前
        }

        public override void OnConfigureServices(ServiceConfigurationContext context)
        {
            // 注册服务
        }

        public override void OnPostConfigureServices(ServiceConfigurationContext context)
        {
            // 注册服务之后
        }

        public override void OnPreApplicationInitialization(ApplicationInitializationContext context)
        {
            // 应用初始化之前
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            // 应用初始化
        }

        public override void OnPostApplicationInitialization(ApplicationInitializationContext context)
        {
            // 应用初始化之后
        }

        public override void OnApplicationShutdown(ApplicationShutdownContext context)
        {
            // 应用停止
        }
    }
}
