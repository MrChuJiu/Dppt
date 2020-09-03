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
        /// 模块  StartupModules 集合 
        /// </summary>
        public ICollection<IAppModule> StartupModules { get; } = new List<IAppModule>();

        public void DiscoverStartupModules()
        {
            DiscoverStartModule(Assembly.GetEntryAssembly()!);
        }

        private void DiscoverStartModule(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0 || assemblies.All(a => a == null))
            {
                throw new ArgumentException("没有从程序及中发现任何模块", nameof(assemblies));
            }

            foreach (var type in assemblies.SelectMany(a => a.ExportedTypes))
            {
                if (typeof(IAppModule).IsAssignableFrom(type))
                {
                    var instance = Activate(type);
                    StartupModules.Add(instance);
                }
            }
        }
        private IAppModule Activate(Type type)
        {
            try
            {
                return (IAppModule)Activator.CreateInstance(type)!;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"未能成功创建实例 {nameof(IAppModule)}  '{type.Name}'.", ex);
            }
        }
    }
}
