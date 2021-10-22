using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dppt.Authorization.Abstractions.Permissions.Permission;
using Dppt.Authorization.Abstractions.Permissions.PermissionValue;

namespace Dppt.Authorization.Permissions
{
    public abstract class PermissionValueProvider : IPermissionValueProvider
    {
        public abstract string Name { get; }

        protected IPermissionStore PermissionStore { get; }

        protected PermissionValueProvider(IPermissionStore permissionStore)
        {
            PermissionStore = permissionStore;
        }

        public abstract Task<PermissionGrantResult> CheckAsync(PermissionValueCheckContext context);

    }
}
