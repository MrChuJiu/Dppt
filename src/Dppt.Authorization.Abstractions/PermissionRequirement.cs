using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace Dppt.Authorization.Abstractions
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string PermissionName { get; }

        public PermissionRequirement([NotNull] string permissionName)
        {
            PermissionName = permissionName;
        }

        public override string ToString()
        {
            return $"PermissionRequirement: {PermissionName}";
        }
    }
}
