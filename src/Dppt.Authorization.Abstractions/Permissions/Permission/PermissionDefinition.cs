using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions
{
    public class PermissionDefinition
    {
        /// <summary>
        /// 唯一的权限标识名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 当前权限的父级权限，这个属性的值只可以通过 AddChild() 方法进行设置。
        /// </summary>
        public PermissionDefinition Parent { get; private set; }

        /// <summary>
        /// 适用的权限值提供者，为空的时候则使用所有的提供者进行校验。
        /// </summary>
        public List<string> Providers { get; }

        ///// <summary>
        ///// 权限状态提供者
        ///// </summary>
        //public List<IPermissionStateProvider> StateProviders { get; }

        /// <summary>
        /// 权限名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 获取权限的子级权限
        /// </summary>
        public IReadOnlyList<PermissionDefinition> Children => _children.ToImmutableList();
        private readonly List<PermissionDefinition> _children;

        /// <summary>
        /// 开发人员针对权限的一些自定义属性。
        /// </summary>
        public Dictionary<string, object> Properties { get; }


        /// <summary>
        /// 禁用/启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 针对于自定义属性的快捷索引器。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get => Properties.GetOrDefault(name);
            set => Properties[name] = value;
        }

        protected internal PermissionDefinition(
            [NotNull] string name,
            string displayName = null,
            bool isEnabled = true)
        {
            Name = name;
            DisplayName = displayName ?? name;
            IsEnabled = isEnabled;

            Properties = new Dictionary<string, object>();
            Providers = new List<string>();
            _children = new List<PermissionDefinition>();
        }


        /// <summary>
        /// 添加子集
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public virtual PermissionDefinition AddChild(
            [NotNull] string name,
            string displayName = null,
            bool isEnabled = true)
        {
            var child = new PermissionDefinition(
                name,
                displayName,
                isEnabled)
            {
                Parent = this
            };

            _children.Add(child);

            return child;
        }


        public override string ToString()
        {
            return $"[{nameof(PermissionDefinition)} {Name}]";
        }


    }
}
