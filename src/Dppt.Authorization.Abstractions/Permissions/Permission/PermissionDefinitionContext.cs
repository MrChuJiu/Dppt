using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public class PermissionDefinitionContext: IPermissionDefinitionContext
    {
        public IServiceProvider ServiceProvider { get; }

        public Dictionary<string, PermissionGroupDefinition> Groups { get; }

        public PermissionDefinitionContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            Groups = new Dictionary<string, PermissionGroupDefinition>();
        }


        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public virtual PermissionGroupDefinition AddGroup(
            string name,
            string displayName = null)
        {

            if (Groups.ContainsKey(name))
            {
                throw new AbpException($"There is already an existing permission group with name: {name}");
            }

            return Groups[name] = new PermissionGroupDefinition(name, displayName);
        }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [NotNull]
        public virtual PermissionGroupDefinition GetGroup([NotNull] string name)
        {
            var group = GetGroupOrNull(name);

            if (group == null)
            {
                throw new AbpException($"Could not find a permission definition group with the given name: {name}");
            }

            return group;
        }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionGroupDefinition GetGroupOrNull([NotNull] string name)
        {

            if (!Groups.ContainsKey(name))
            {
                return null;
            }

            return Groups[name];
        }

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="name"></param>
        public virtual void RemoveGroup(string name)
        {
            if (!Groups.ContainsKey(name))
            {
                throw new AbpException($"Not found permission group with name: {name}");
            }

            Groups.Remove(name);
        }

        /// <summary>
        /// 从所有权限组中 获取指定的权限名权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual PermissionDefinition GetPermissionOrNull([NotNull] string name)
        {
            foreach (var groupDefinition in Groups.Values)
            {
                var permissionDefinition = groupDefinition.GetPermissionOrNull(name);

                if (permissionDefinition != null)
                {
                    return permissionDefinition;
                }
            }

            return null;
        }

    }
}
