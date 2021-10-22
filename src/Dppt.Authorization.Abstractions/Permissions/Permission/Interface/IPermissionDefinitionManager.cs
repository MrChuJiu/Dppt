using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public interface IPermissionDefinitionManager
    {
        [NotNull]
        PermissionDefinition Get([NotNull] string name);

        [CanBeNull]
        PermissionDefinition GetOrNull([NotNull] string name);

        IReadOnlyList<PermissionDefinition> GetPermissions();

        IReadOnlyList<PermissionGroupDefinition> GetGroups();
    }
}
