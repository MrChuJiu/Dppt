using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

        }

        /// <summary>
        /// 模块排序
        /// </summary>
        /// <typeparam name="TModule"></typeparam>
        /// <returns></returns>
        private List<ModuleDescriptor> ModuleSort<TModule>() where TModule : IAppModule
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
    }
}
