using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    public class ModuleManager : IModuleManager
    {
        /// <summary>
        /// 模块接口类型全名称
        /// </summary>
        public static string _moduleInterfaceTypeFullName = typeof(IAppModule).FullName;
        /// <summary>
        /// 模块明细和实例   (存储 系统所有模块 和 对应的依赖模块)
        /// </summary>
        public virtual IReadOnlyList<ModuleDescriptor> ModuleDescriptors { get; protected set; }
        /// <summary>
        /// ioc容器
        /// </summary>
        public virtual IServiceProvider ServiceProvider { get; protected set; }



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

        public void ApplicationShutdown()
        {
            var context = new ApplicationShutdownContext(this.ServiceProvider);

            var modules = ModuleDescriptors.Reverse().ToList();
            foreach (var module in modules)
            {
                (module as IAppModule)?.OnApplicationShutdown(context);
            }

        }

        public IServiceCollection ConfigurationService(IServiceCollection services, IConfiguration configuration)
        {
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


            services.AddSingleton(context);

            return services;
        }
        public List<ModuleDescriptor> ModuleSort<TModule>() where TModule : IAppModule
        {
            var moduleDescriptors = VisitModule(typeof(TModule));

            return Topological.Sort(moduleDescriptors, o => o.Dependencies);
        }

        protected virtual List<ModuleDescriptor> VisitModule(Type moduleType) {

            var moduleDescriptors = new List<ModuleDescriptor>();

            // 过滤抽象类、接口、泛型类、非类
            if (moduleType.IsAbstract
                || moduleType.IsInterface
                || moduleType.IsGenericType
                || !moduleType.IsClass)
            {
                return moduleDescriptors;
            }

            //过滤没有实现  IAppModule 的 类
            var baseInterfaceType = moduleType.GetInterface(_moduleInterfaceTypeFullName, false);
            if (baseInterfaceType == null)
            {
                return moduleDescriptors;
            }
            // 反射获得自定义属性
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


        public void StartModule<TModule>(IServiceCollection services) where TModule : IAppModule
        {
            var moduleDescriptors = new List<ModuleDescriptor>();

            var moduleDescriptorList = this.ModuleSort<TModule>();

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
