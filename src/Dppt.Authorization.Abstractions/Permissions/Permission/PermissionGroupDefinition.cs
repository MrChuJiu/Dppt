using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions
{
    public class PermissionGroupDefinition
    {
        /// <summary>
        /// 唯一的权限标识名称。
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 权限名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 开发人员针对权限的一些自定义属性。
        /// </summary>
        public Dictionary<string, object> Properties { get; }


        /// <summary>
        /// 权限组内的权限
        /// </summary>
        public IReadOnlyList<PermissionDefinition> Permissions => _permissions.ToImmutableList();
        private readonly List<PermissionDefinition> _permissions;

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



        protected internal PermissionGroupDefinition(
            string name,
            string displayName = null)
        {
            Name = name;
            DisplayName = displayName ?? name;

            Properties = new Dictionary<string, object>();
            _permissions = new List<PermissionDefinition>();
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <param name="isEnabled"></param>
        /// <returns></returns>
        public virtual PermissionDefinition AddPermission(
            string name,
            string displayName = null,
            bool isEnabled = true)
        {
            var permission = new PermissionDefinition(
                name,
                displayName,
                isEnabled
            );

            _permissions.Add(permission);

            return permission;
        }

        /// <summary>
        /// 递归构建权限集合，因为定义的某个权限内部还拥有子权限。
        /// </summary>
        /// <returns></returns>
        public virtual List<PermissionDefinition> GetPermissionsWithChildren()
        {
            var permissions = new List<PermissionDefinition>();

            foreach (var permission in _permissions)
            {
                AddPermissionToListRecursively(permissions, permission);
            }

            return permissions;
        }


        /// <summary>
        /// 递归构建方法。
        /// </summary>
        /// <param name="permissions"></param>
        /// <param name="permission"></param>
        private void AddPermissionToListRecursively(List<PermissionDefinition> permissions, PermissionDefinition permission)
        {
            permissions.Add(permission);

            foreach (var child in permission.Children)
            {
                AddPermissionToListRecursively(permissions, child);
            }
        }



        [CanBeNull]
        public PermissionDefinition GetPermissionOrNull([NotNull] string name)
        {
            return GetPermissionOrNullRecursively(Permissions, name);
        }


        private PermissionDefinition GetPermissionOrNullRecursively(
            IReadOnlyList<PermissionDefinition> permissions, string name)
        {
            foreach (var permission in permissions)
            {
                if (permission.Name == name)
                {
                    return permission;
                }

                var childPermission = GetPermissionOrNullRecursively(permission.Children, name);
                if (childPermission != null)
                {
                    return childPermission;
                }
            }

            return null;
        }


        public override string ToString()
        {
            return $"[{nameof(PermissionGroupDefinition)} {Name}]";
        }



    }
}
