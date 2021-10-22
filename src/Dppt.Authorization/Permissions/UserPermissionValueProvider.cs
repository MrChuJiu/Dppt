using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;
using Dppt.Security.Claims;

namespace Dppt.Authorization.Permissions
{
    public class UserPermissionValueProvider : PermissionValueProvider
    {
        public const string ProviderName = "U";

        public override string Name => ProviderName;

        public UserPermissionValueProvider(IPermissionStore permissionStore)
            : base(permissionStore)
        {

        }

        public override async Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context)
        {
            var userId = context.Principal?.FindFirst(AbpClaimTypes.UserId)?.Value;

            if (userId == null)
            {
                return PermissionGrantResult.Undefined;
            }

            return await PermissionStore.IsGrantedAsync(context.Permission.Name, Name, userId)
                ? PermissionGrantResult.Granted
                : PermissionGrantResult.Undefined;
        }
    }
}
