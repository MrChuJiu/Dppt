using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Easy.Core.Flow.StartupModules
{
    public class StartupModulesOptions
    {
        /// <summary>
        /// 存储实验了IStartupModule接口的模块
        /// </summary>
        public IList<IStartupModule> StartupModules { get; private set; }

        /// <summary>
        /// 检索当前项目启动模块
        /// </summary>
        public void DiscoverStartupModules() => DiscoverStartupModules(Assembly.GetEntryAssembly()!);

        public void DiscoverStartupModules(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0 || assemblies.All(a => a == null))
            {
                throw new ArgumentException("没有发现任何模块", nameof(assemblies));
            }

            // 是否必须被重写|是否是接口|是否为泛型类型|是否是一个类或委托
            StartupModules = assemblies.SelectMany(a => a.ExportedTypes)
                .Where(s =>
                !(s.IsAbstract || s.IsInterface || s.IsGenericType || !s.IsClass) &&
                typeof(IStartupModule).IsAssignableFrom(s)).Select(s => Activate(s)).ToList();
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IStartupModule Activate(Type type)
        {
            try
            {
                return (IStartupModule)Activator.CreateInstance(type)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"创建实例为 {nameof(IStartupModule)} 的实例失败 Name：'{type.Name}'.", ex);
            }
        }
    }
}
