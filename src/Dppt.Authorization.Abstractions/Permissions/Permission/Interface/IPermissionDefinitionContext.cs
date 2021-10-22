using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public interface IPermissionDefinitionContext
    {
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PermissionGroupDefinition GetGroup([NotNull] string name);

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [NotNull]
        PermissionGroupDefinition GetGroupOrNull(string name);

        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        /// <returns></returns>
        [CanBeNull]
        PermissionGroupDefinition AddGroup(
            [NotNull] string name,
            string displayName = null);

        /// <summary>
        /// 删除权限组
        /// </summary>
        /// <param name="name"></param>
        void RemoveGroup(string name);

        /// <summary>
        /// 从所有权限组中 获取指定的权限名权限
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [CanBeNull]
        PermissionDefinition GetPermissionOrNull([NotNull] string name);
    }
}
