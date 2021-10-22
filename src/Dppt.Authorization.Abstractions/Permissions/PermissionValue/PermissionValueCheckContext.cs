using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.PermissionValue
{
    public class PermissionValueCheckContext
    {
        [NotNull]
        public PermissionDefinition Permission { get; }

        [CanBeNull]
        public ClaimsPrincipal Principal { get; }

        public PermissionValueCheckContext(
            [NotNull] PermissionDefinition permission,
            [CanBeNull] ClaimsPrincipal principal)
        {
        
            Permission = permission;
            Principal = principal;
        }
    }
}
