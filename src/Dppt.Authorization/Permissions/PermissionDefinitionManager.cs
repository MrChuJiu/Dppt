using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Dppt.Authorization.Permissions
{
    public class PermissionDefinitionManager: IPermissionDefinitionManager
    {
        protected IDictionary<string, PermissionGroupDefinition> PermissionGroupDefinitions => _lazyPermissionGroupDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionGroupDefinition>> _lazyPermissionGroupDefinitions;

        protected IDictionary<string, PermissionDefinition> PermissionDefinitions => _lazyPermissionDefinitions.Value;
        private readonly Lazy<Dictionary<string, PermissionDefinition>> _lazyPermissionDefinitions;

        protected AbpPermissionOptions Options { get; }

        private readonly IServiceProvider _serviceProvider;


        public PermissionDefinitionManager(IServiceProvider serviceProvider,
            IOptions<AbpPermissionOptions> options)
        {
            _serviceProvider = serviceProvider;

            Options = options.Value;

            _lazyPermissionDefinitions = new Lazy<Dictionary<string, PermissionDefinition>>(
                CreatePermissionDefinitions,
                isThreadSafe: true
            );

            _lazyPermissionGroupDefinitions = new Lazy<Dictionary<string, PermissionGroupDefinition>>(
                CreatePermissionGroupDefinitions,
                isThreadSafe: true
            );
        }


        public PermissionDefinition Get(string name)
        {
            var permission = GetOrNull(name);

            if (permission == null)
            {
                throw new AbpException("Undefined permission: " + name);
            }

            return permission;
        }

        public PermissionDefinition GetOrNull(string name)
        {
            return PermissionDefinitions.GetOrDefault(name);
        }

        public IReadOnlyList<PermissionDefinition> GetPermissions()
        {
            return PermissionDefinitions.Values.ToImmutableList();
        }

        public IReadOnlyList<PermissionGroupDefinition> GetGroups()
        {
            return PermissionGroupDefinitions.Values.ToImmutableList();
        }

        /// <summary>
        /// 获取所有权限平面化数据
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionDefinition> CreatePermissionDefinitions()
        {
            var permissions = new Dictionary<string, PermissionDefinition>();

            foreach (var groupDefinition in PermissionGroupDefinitions.Values)
            {
                foreach (var permission in groupDefinition.Permissions)
                {
                    AddPermissionToDictionaryRecursively(permissions, permission);
                }
            }

            return permissions;
        }



        protected virtual void AddPermissionToDictionaryRecursively(
            Dictionary<string, PermissionDefinition> permissions,
            PermissionDefinition permission)
        {
            if (permissions.ContainsKey(permission.Name))
            {
                throw new AbpException("Duplicate permission name: " + permission.Name);
            }

            permissions[permission.Name] = permission;

            foreach (var child in permission.Children)
            {
                AddPermissionToDictionaryRecursively(permissions, child);
            }
        }


        /// <summary>
        /// 通过获取提供者的实现来获得所有权限组数据
        /// </summary>
        /// <returns></returns>
        protected virtual Dictionary<string, PermissionGroupDefinition> CreatePermissionGroupDefinitions()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = new PermissionDefinitionContext(scope.ServiceProvider);

                var providers = Options
                    .DefinitionProviders
                    .Select(p => scope.ServiceProvider.GetRequiredService(p) as IPermissionDefinitionProvider)
                    .ToList();

                foreach (var provider in providers)
                {
                    provider.PreDefine(context);
                }

                foreach (var provider in providers)
                {
                    provider.Define(context);
                }

                foreach (var provider in providers)
                {
                    provider.PostDefine(context);
                }

                return context.Groups;
            }
        }
    }
}
