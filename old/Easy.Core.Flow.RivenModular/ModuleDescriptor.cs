using System;
using System.Collections.Generic;
using System.Text;

namespace Easy.Core.Flow.RivenModular
{
    /// <summary>
    /// 模块描述
    /// </summary>
    public class ModuleDescriptor
    {
        private object _instance;

        /// <summary>
        /// 模块类型
        /// </summary>
        public Type ModuleType { get; private set; }

        /// <summary>
        /// 依赖项
        /// </summary>
        public ModuleDescriptor[] Dependencies { get; private set; }

        /// <summary>
        /// 实例
        /// </summary>
        public object Instance
        {
            get
            {
                if (this._instance == null)
                {
                    this._instance = Activator.CreateInstance(this.ModuleType);
                }
                return this._instance;
            }
        }

        public ModuleDescriptor(Type moduleType, params ModuleDescriptor[] dependencies)
        {
            this.ModuleType = moduleType;
            // 如果模块依赖 为空给一个空数组
            this.Dependencies = dependencies ?? new ModuleDescriptor[0];
        }
    }
}
