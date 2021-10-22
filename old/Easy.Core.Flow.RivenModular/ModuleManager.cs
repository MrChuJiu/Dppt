using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Easy.Core.Flow.RivenModular
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    public class ModuleManager : IModuleManager
    {
        /// <summary>
        /// 模块接口类型全名称
        /// </summary>
        public static string _moduleInterfaceTypeFullName = typeof(IAppModule).FullName;
        /// <summary>
        /// 模块明细和实例
        /// </summary>
        public virtual IReadOnlyList<ModuleDescriptor> ModuleDescriptors { get; protected set; }
        /// <summary>
        /// ioc容器
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; protected set; }


        /// <summary>
        /// 入口 StartModule 
        /// 我们通过传递泛型进来的 TModule 为起点
        /// 查找他的依赖树
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <param name="services"></param>
        public void StartModule<TModule>(IServiceCollection services) where TModule : IAppModule
        {

            var moduleDescriptors = new List<ModuleDescriptor>();

            var moduleDescriptorList = this.ModuleSort<TModule>();
            // 去除重复的引用 进行注入
            foreach (var item in moduleDescriptorList)
            {
                if (moduleDescriptors.Any(o => o.ModuleType.FullName == item.ModuleType.FullName))
                {
                    continue;
                }
                moduleDescriptors.Add(item);
                services.AddSingleton(item.ModuleType, item.Instance);
            }
            ModuleDescriptors = moduleDescriptors.AsReadOnly();
        }

        /// <summary>
        /// 进行模块的  ConfigurationService 方法调用
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public IServiceCollection ConfigurationService(IServiceCollection services, IConfiguration configuration) {

            var context = new ServiceConfigurationContext(services, configuration);

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPreConfigureServices(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnConfigureServices(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPostConfigureServices(context);
            }

            return services;
        }
        /// <summary>
        /// 进行模块的  Configure 方法调用
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public IServiceProvider ApplicationInitialization(IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetService<IConfiguration>();
            var context = new ApplicationInitializationContext(serviceProvider, configuration);

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPreApplicationInitialization(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnApplicationInitialization(context);
            }

            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnPostApplicationInitialization(context);
            }
            this.ServiceProvider = serviceProvider;
            return serviceProvider;
        }
        /// <summary>
        /// 模块销毁
        /// </summary>
        public void ApplicationShutdown()
        {

            var context = new ApplicationShutdownContext(this.ServiceProvider);
            var modules = ModuleDescriptors.Reverse().ToList();


            foreach (var module in ModuleDescriptors)
            {
                (module.Instance as IAppModule)?.OnApplicationShutdown(context);
            }
        }

        /// <summary>
        /// 模块排序
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        public virtual List<ModuleDescriptor> ModuleSort<TModule>() where TModule : IAppModule
        {
            // 得到模块树依赖
            var moduleDescriptors = VisitModule(typeof(TModule));
            // 因为现在得到的数据是从树根开始到树叶 - 实际的注入顺序应该是从树叶开始 所以这里需要对模块进行排序
            return Topological.Sort(moduleDescriptors, o => o.Dependencies);
        }


        /// <summary>
        /// 获取模块依赖树
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        protected virtual List<ModuleDescriptor> VisitModule(Type moduleType) {

            var moduleDescriptors = new List<ModuleDescriptor>();
            // 是否必须被重写|是否是接口|是否为泛型类型|是否是一个类或委托
            if (moduleType.IsAbstract || moduleType.IsInterface || moduleType.IsGenericType || !moduleType.IsClass) {
                return moduleDescriptors;
            }

            // 过滤没有实现IRModule接口的类
            var baseInterfaceType = moduleType.GetInterface(_moduleInterfaceTypeFullName, false);
            if (baseInterfaceType == null)
            {
                return moduleDescriptors;
            }

            // 得到当前模块依赖了那些模块
            var dependModulesAttribute = moduleType.GetCustomAttribute<DependsOnAttribute>();
            // 依赖属性为空
            if (dependModulesAttribute == null)
            {
                moduleDescriptors.Add(new ModuleDescriptor(moduleType));
            }
            else {
                // 依赖属性不为空,递归获取依赖
                var dependModuleDescriptors = new List<ModuleDescriptor>();
                foreach (var dependModuleType in dependModulesAttribute.DependModuleTypes)
                {
                    dependModuleDescriptors.AddRange(
                        VisitModule(dependModuleType)
                    );
                }
                // 创建模块描述信息,内容为模块类型和依赖类型
                moduleDescriptors.Add(new ModuleDescriptor(moduleType, dependModuleDescriptors.ToArray()));
            }

            return moduleDescriptors;
        }

        /// <summary>
        /// 主模块销毁的时候 销毁子模块
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool state)
        {
            this.ApplicationShutdown();

        }


    }
}
