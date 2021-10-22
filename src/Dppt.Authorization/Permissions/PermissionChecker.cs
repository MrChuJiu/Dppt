using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionChecker.Interface;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;
using Dppt.Security.Dppt.Security.Claims;

namespace Dppt.Authorization.Permissions
{
    public class PermissionChecker : IPermissionChecker
    {
        protected IPermissionDefinitionManager PermissionDefinitionManager { get; }
        protected ICurrentPrincipalAccessor PrincipalAccessor { get; }
        protected IPermissionValueProviderManager PermissionValueProviderManager { get; }

        public PermissionChecker(
            ICurrentPrincipalAccessor principalAccessor,
            IPermissionDefinitionManager permissionDefinitionManager,
            IPermissionValueProviderManager permissionValueProviderManager
            )
        {
            PrincipalAccessor = principalAccessor;
            PermissionDefinitionManager = permissionDefinitionManager;
           PermissionValueProviderManager = permissionValueProviderManager;
        }


        public virtual async Task<bool> IsGrantedAsync(string name)
        {
            return await IsGrantedAsync(PrincipalAccessor.Principal, name);
        }

        public virtual async Task<bool> IsGrantedAsync(
            ClaimsPrincipal claimsPrincipal,
            string name)
        {
  
            var permission = PermissionDefinitionManager.Get(name);
            // 判断权限是否关闭
            if (!permission.IsEnabled)
            {
                return false;
            }

            var isGranted = false;
            // 针对不同维度的权限检测。
            var context = new PermissionValueCheckContext(permission, claimsPrincipal);
            foreach (var provider in PermissionValueProviderManager.ValueProviders)
            {
                if (context.Permission.Providers.Any() &&
                    !context.Permission.Providers.Contains(provider.Name))
                {
                    continue;
                }

                var result = await provider.CheckAsync(context);

                if (result == PermissionGrantResult.Granted)
                {
                    isGranted = true;
                }
                else if (result == PermissionGrantResult.Prohibited)
                {
                    return false;
                }
            }

            return isGranted;
        }

    }
}
