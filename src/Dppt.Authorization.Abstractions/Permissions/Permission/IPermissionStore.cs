using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Dppt.Authorization.Abstractions.Permissions.Permission
{
    public interface IPermissionStore
    {
        Task<bool> IsGrantedAsync(
            [NotNull] string name,
            [CanBeNull] string providerName,
            [CanBeNull] string providerKey
        );
    }
}
